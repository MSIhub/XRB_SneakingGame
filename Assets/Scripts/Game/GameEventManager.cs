using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameEventManager : MonoBehaviour
    {
        public enum GameMode
        {
            FP = 0,
            VR = 1
        }
        [Header("Accessibility")] 
        public Handed handedness;
        public GameMode gameMode;

        [Header("UI")] [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _failedPanel;
        [SerializeField] private GameObject _successPanel;
        [SerializeField] private float _canvasFadeTime = 2f;
        [SerializeField] private Material _skyboxMaterial;

        [Header("Audio")] [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioClip _caughtMusic;
        [SerializeField] private AudioClip _successMusic;

        private PlayerInput _playerInput;
        private FirstPersonController _fpController;
        private bool _isFadingIn = false;
        private float _fadeLevel = 0f;
        private bool _isGoalReached = false;
        
        private float _initialSkyboxAtmosphereThickness;
        private Color _initialSkyboxColor;
        private float _initialSkyboxExposure;

        private void Awake()
        {
            handedness = (Handed) PlayerPrefs.GetInt("handedness");
        }

        private void Start()
        {
            EnemyController[]
                enemies = FindObjectsOfType<EnemyController>(); //Unoptimized way of handling this. Only once used.
            foreach (EnemyController enemy in enemies)
            {
                enemy.onInvestigate.AddListener(EnemyInvestigating);
                enemy.onPlayerFound.AddListener(PlayerFound);
                enemy.onReturnToPatrol.AddListener(EnemyReturnToPatrol);
            }

            GameObject player = GameObject.FindWithTag("Player"); //UnOptimized way
            if (player)
            {
                _playerInput = player.GetComponent<PlayerInput>();
                _fpController = player.GetComponent<FirstPersonController>();
            }
            else
            {
                Debug.LogWarning("There is no player in the scene");
            }


            //Setting the fail and success canvas to not show on the start of the game
            _canvasGroup.alpha = 0;
            _failedPanel.SetActive(false);
            _successPanel.SetActive(false);
            
            
            ResetShaderValues();
            _initialSkyboxAtmosphereThickness = _skyboxMaterial.GetFloat("_AtmosphereThinkness");
            _initialSkyboxColor = _skyboxMaterial.GetColor("_SkyTint");
            _initialSkyboxExposure = _skyboxMaterial.GetFloat("_Exposure");
            
           
        }

        private void EnemyReturnToPatrol()
        {
        }

        private void PlayerFound(Transform enemyThatFoundPlayer) //mission failed method
        {
            if (_isGoalReached) return;
            _failedPanel.SetActive(true);
            if (gameMode == GameMode.FP)
            {
                _isFadingIn = true;
                
                // if player found, making the player look at the enemy
                _fpController.CinemachineCameraTarget.transform.LookAt(enemyThatFoundPlayer);
                DeactivateInput();
            }
            else
            {
                
                StartCoroutine((GameOverFadeOutSaturation(1f)));
            }
            
            
            PlayBGM(_caughtMusic);
        }

        private IEnumerator GameOverFadeOutSaturation(float startDelay = 0f)
        {
            yield return new WaitForSeconds(startDelay);
            Time.timeScale = 0;
            float fade = 0f;
            while (fade < 1)
            {
                fade += Time.unscaledDeltaTime/_canvasFadeTime; //As time.timescale is 0, the game basically pauses. Thus this is the way to extract deltatime.
                Shader.SetGlobalFloat("_AllWhite", fade); // GlobalFLoat can be edited by code and not exposed in the editor.
                _skyboxMaterial.SetFloat("_AtmosphereThinkness",Mathf.Lerp(_initialSkyboxAtmosphereThickness,0.7f,fade ));
                _skyboxMaterial.SetColor("_SkyTint",Color.Lerp(_initialSkyboxColor,Color.white,fade));
                _skyboxMaterial.SetFloat("_Exposure",Mathf.Lerp(_initialSkyboxExposure,8f,fade ));
                yield return null;
            }

            yield return new WaitForSecondsRealtime(2f);
            RestartScene();
        }

        private void OnDestroy()
        {
            ResetShaderValues();
        }

        private void ResetShaderValues()
        {
            Shader.SetGlobalFloat("_AllWhite", 0f);
            _skyboxMaterial.SetFloat("_AtmosphereThinkness",_initialSkyboxAtmosphereThickness);
            _skyboxMaterial.SetColor("_SkyTint",_initialSkyboxColor);
            _skyboxMaterial.SetFloat("_Exposure",_initialSkyboxExposure);
        }

        public void GoalReached() //mission success method
        {
            _isFadingIn = true;
            _isGoalReached = true;
            _successPanel.SetActive(true);
            DeactivateInput();
            PlayBGM(_successMusic);
        }


        private void DeactivateInput()
        {
            //Deactivated player input
            _playerInput.DeactivateInput();
            //unlock the cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void PlayBGM(AudioClip newBgm)
        {
            if (_bgmSource.clip == newBgm) return;
            _bgmSource.clip = newBgm;
            _bgmSource.Play();
        }

        private void EnemyInvestigating()
        {
        }

        public void RestartScene()
        {
            //unlock the cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            ResetShaderValues();
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (_isFadingIn)
            {
                if (_fadeLevel < 1f)
                {
                    if (_canvasFadeTime > 0f)
                    {
                        _fadeLevel += Time.deltaTime / _canvasFadeTime;
                    }
                }
            }
            else
            {
                if (_fadeLevel > 0f)
                {
                    if (_canvasFadeTime > 0f)
                    {
                        _fadeLevel -= Time.deltaTime / _canvasFadeTime;
                    }
                }
            }

            _canvasGroup.alpha = _fadeLevel;
        }

        public void ToggleDominantHand()
        {
            handedness = handedness == Handed.Right ? Handed.Left : Handed.Right;
            PlayerPrefs.SetInt("handedness", (int) handedness); //Saving playerpreferances over multiple passthroughs
            PlayerPrefs.Save();
        }
    }
}