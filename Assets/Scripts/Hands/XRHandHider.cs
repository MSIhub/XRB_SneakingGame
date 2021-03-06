using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
    public class XRHandHider : MonoBehaviour
    {
        [SerializeField] private XRBaseControllerInteractor _controller;
        [SerializeField] private Rigidbody _handRigidBody;
        [SerializeField] private ConfigurableJoint _configJoint;
        [SerializeField] private float _handShowDelay = 0.15f;
        
        private Quaternion _originalHandRot;
        
        // Start is called before the first frame update
        void Start()
        {
            _controller.selectEntered.AddListener(SelectEntered);
            _controller.selectExited.AddListener(SelectExited);

            //_originalHandRot = _handRigidBody.transform.localRotation;//
            _originalHandRot = _configJoint.transform.localRotation;//
        }
        private void SelectEntered(SelectEnterEventArgs arg0)
        {
            if (arg0.interactable is BaseTeleportationInteractable) return;//is type comparision
            _configJoint.gameObject.SetActive(false);
            //_handRigidBody.gameObject.SetActive(false);
            _configJoint.connectedBody = null;
            CancelInvoke(nameof(ShowHand));
            
        }
        private void SelectExited(SelectExitEventArgs arg0)
        {
            if (arg0.interactable is BaseTeleportationInteractable) return;
            Invoke(nameof(ShowHand), _handShowDelay);
        }

        private void ShowHand()
        {
            _configJoint.gameObject.SetActive(true);
           //_handRigidBody.gameObject.SetActive(true);
           //_handRigidBody.transform.position = _controller.transform.position;//
           //_handRigidBody.transform.rotation = Quaternion.Euler(_controller.transform.eulerAngles + _originalHandRot.eulerAngles);//
           _configJoint.transform.position = _controller.transform.position;//
           _configJoint.transform.rotation = Quaternion.Euler(_controller.transform.eulerAngles + _originalHandRot.eulerAngles);
           _configJoint.connectedBody = _handRigidBody;
        }
        
    }
}