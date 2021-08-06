using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameEventManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _failedPanel;
        [SerializeField] private GameObject _successPanel;
        [SerializeField] private float _canvasFadeTime = 2f;

        [Header("Audio")] 
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioClip _caughtMusic;
        
        private PlayerInput _playerInput;
        private FirstPersonController _fpController;
        private bool _isFadingIn = false;
        private float _fadeLevel = 0f;

        private void Start()
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>(); //Unoptimized way of handling this. Only once used.
            foreach (EnemyController enemy in enemies)
            {
                enemy.onInvestigate.AddListener(EnemyInvestigating);
                enemy.onPlayerFound.AddListener(PlayerFound);
                enemy.onReturnToPatrol.AddListener(EnemyReturnToPatrol);
            }

            GameObject player = GameObject.FindWithTag("Player");//UnOptimized way
            if (player)
            {
                _playerInput = player.GetComponent<PlayerInput>();    
                _fpController = player.GetComponent<FirstPersonController>();
            }
            else
            {
                Debug.LogWarning("There is no player in the scene");
            }
            
        }

        private void EnemyReturnToPatrol()
        {
            
        }

        private void PlayerFound(Transform enemyThatFoundPlayer)
        {
            _isFadingIn = true;
            // if player found, making the player look at the enemy
            _fpController.CinemachineCameraTarget.transform.LookAt(enemyThatFoundPlayer);
            DeactivateInput();
            PlayBGM(_caughtMusic);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Update()
        {
            if (_isFadingIn)
            {
                if (_fadeLevel < 1f)
                {
                    if (_canvasFadeTime > 0f)
                    {
                        _fadeLevel += Time.deltaTime/_canvasFadeTime;
                    }
                }
            }
            else
            {
                if (_fadeLevel > 0f)
                {
                    if (_canvasFadeTime > 0f)
                    {
                        _fadeLevel -= Time.deltaTime/_canvasFadeTime;    
                    }
                }
            }
            _canvasGroup.alpha = _fadeLevel;
        }
        
        
    }
}