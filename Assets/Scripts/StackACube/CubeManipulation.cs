using UnityEngine;
using UnityEngine.InputSystem;

namespace StackACube
{
    public class CubeManipulation : MonoBehaviour
    {
        [SerializeField] private InputActionReference _leftPrimaryButtonReference;
        [SerializeField] private InputActionReference _rightPrimaryButtonReference;
        [SerializeField] private GameObject _instantiateMesh;
        [SerializeField] private InputActionReference _leftPosition;
        [SerializeField] private InputActionReference _leftRotation;
        

        private Vector3 _leftControllerPositon;
        private Quaternion _leftControllerRotation;
        
        // Start is called before the first frame update
        void Start()
        {
            
            _leftPrimaryButtonReference.action.performed += OnLeftPrimaryButtonPressed;
            _rightPrimaryButtonReference.action.performed += OnRightPrimaryButtonPressed;

        }
        private void OnLeftPrimaryButtonPressed(InputAction.CallbackContext obj)
        {
            Debug.Log("Left ButtonPressed");
           Instantiate(_instantiateMesh, _leftControllerPositon, _leftControllerRotation);
        }
        private void OnRightPrimaryButtonPressed(InputAction.CallbackContext obj)
        {
            Debug.Log("Right ButtonPressed");
        }

      

        // Update is called once per frame
        void Update()
        {
            _leftControllerPositon = transform.position + _leftPosition.action.ReadValue<Vector3>() ;
            _leftControllerRotation = _leftRotation.action.ReadValue<Quaternion>();
            Debug.Log(_leftControllerPositon);
        }
    }
}
