using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameEventManager : MonoBehaviour
    {
        [Header("Accessibility")] 
        public Handed handedness;
        
        [Header("UI")] [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _failedPanel;
        [SerializeField] private GameObject _successPanel;
        [SerializeField] private float _canvasFadeTime = 2f;

        [Header("Audio")] [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioClip _caughtMusic;
        [SerializeField] private AudioClip _successMusic;

        private PlayerInput _playerInput;
        private FirstPersonController _fpController;
        private bool _isFadingIn = false;
        private float _fadeLevel = 0f;
        private bool _isGoalReached = false;

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
        }

        private void EnemyReturnToPatrol()
        {
        }

        private void PlayerFound(Transform enemyThatFoundPlayer) //mission failed method
        {
            if (_isGoalReached) return;
            _isFadingIn = true;
            _failedPanel.SetActive(true);
            // if player found, making the player look at the enemy
            _fpController.CinemachineCameraTarget.transform.LookAt(enemyThatFoundPlayer);
            DeactivateInput();
            PlayBGM(_caughtMusic);
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
    }
}