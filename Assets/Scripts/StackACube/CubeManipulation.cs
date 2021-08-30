using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace StackACube
{
    public class CubeManipulation : MonoBehaviour
    {
        [SerializeField] private GameObject _instantiateMesh;
        [SerializeField] private InputActionReference _leftPrimaryButtonReference;
        [SerializeField] private InputActionReference _rightPrimaryButtonReference;
        [SerializeField] private InputActionReference _rightSecondaryButtonReference;
        [SerializeField] private InputActionReference _leftPosition;
        [SerializeField] private InputActionReference _leftRotation;

        private Vector3 _leftControllerPosition;
        private Quaternion _leftControllerRotation;
        private XRGrabInteractable _grabMesh;
        
        // Start is called before the first frame update
        void Start()
        {
            _leftPrimaryButtonReference.action.performed += OnLeftPrimaryButtonPressed;

        }
        
        private void OnLeftPrimaryButtonPressed(InputAction.CallbackContext obj)
        {
            Instantiate(_instantiateMesh, _leftControllerPosition, _leftControllerRotation);
        }


        public void EnableScaleMesh(XRGrabInteractable grabMesh)
        {
            _grabMesh = grabMesh;
            _rightPrimaryButtonReference.action.performed += OnRightPrimaryButtonPressed;
            _rightSecondaryButtonReference.action.performed += OnRightSecondaryButtonPressed;

        }

        public void DisableScaleMesh(XRGrabInteractable grabMesh)
        {
            _rightPrimaryButtonReference.action.performed -= OnRightPrimaryButtonPressed;
            _rightSecondaryButtonReference.action.performed -= OnRightSecondaryButtonPressed;
        }

        
        private void OnRightPrimaryButtonPressed(InputAction.CallbackContext obj)
        {
            _grabMesh.gameObject.transform.localScale += Vector3.one*0.01f;
        }
        
        private void OnRightSecondaryButtonPressed(InputAction.CallbackContext obj)
        {
            _grabMesh.gameObject.transform.localScale -= Vector3.one*0.01f;
        }



      

        // Update is called once per frame
        void Update()
        {
            _leftControllerPosition = transform.TransformPoint(_leftPosition.action.ReadValue<Vector3>()) ;
            _leftControllerRotation = _leftRotation.action.ReadValue<Quaternion>();//rotation is in body frame so no need to transform it
        }
    }
}
