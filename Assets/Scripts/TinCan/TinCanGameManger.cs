using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace TinCan
{
    public class TinCanGameManger : MonoBehaviour
    {
        [SerializeField] private GameObject _canParentObject;
        [SerializeField] private GameObject _ballObjectParent;
        [SerializeField] private GameObject _defaultMessage;
        [SerializeField] private GameObject _congratMessage;
        [SerializeField] private GameObject _failedMessage;
        [SerializeField] private float _waitTime = 1f;

        public int Count { get; set; }
    
        private CanInteractor[] _canPyramid;
        private int _totalBalls = 0;
        private float _waitTimer = 0;
        
        
        //[SerializeField] private bool _allCansFeel = false;
        // Start is called before the first frame update
        private void Start()
        {
            Count = 0;
            DisplayDefaultMessage();
            _canPyramid = _canParentObject.GetComponentsInChildren<CanInteractor>();
            if (_canPyramid.Length < 1)
            {
                Debug.LogWarning("Can pyramid is missing");
            }

            var balls = _ballObjectParent.GetComponentsInChildren<XRGrabInteractable>();
            _totalBalls = balls.Length;
            Debug.Log(_totalBalls);
        }

    
        // Update is called once per frame
        private void Update()
        {
            if (CheckAllCansFeel())
            {
                DisplayCongratsMessage();    
            }
            else if (Count == _totalBalls)
            {
                _waitTimer += Time.deltaTime;
                if(_waitTimer > _waitTime && !CheckAllCansFeel())
                    DisplayFailedMessage();
            }
        }
    
        public void Counter()
        {
            Count += 1;
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        private bool CheckAllCansFeel()
        {
            int check = 0;
            foreach (var can in _canPyramid)
            {
                if (can.canFeel == true)
                    check += 1;
            }

            return (check == _canPyramid.Length);
            
        }

        private void DisplayDefaultMessage()
        {
            _defaultMessage.SetActive(true);
            _congratMessage.SetActive(false);
            _failedMessage.SetActive(false);
        }
        private void DisplayFailedMessage()
        {
            _defaultMessage.SetActive(false);
            _congratMessage.SetActive(false);
            _failedMessage.SetActive(true);
        }

        private void DisplayCongratsMessage()
        {
            _defaultMessage.SetActive(false);
            _congratMessage.SetActive(true);
            _failedMessage.SetActive(false);
        }
    }
}
