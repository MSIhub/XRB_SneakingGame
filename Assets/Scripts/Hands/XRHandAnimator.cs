using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
    public class XRHandAnimator : MonoBehaviour
    {
        [SerializeField] private ActionBasedController _controller;
        [SerializeField] private Animator _animator;
        // Start is called before the first frame update
        private void Start()
        {
            _controller.selectAction.action.started += Point;
            _controller.selectAction.action.canceled += PointReleased;
            
            
            _controller.activateAction.action.started += Fist;
            _controller.activateAction.action.canceled += FistReleased;
        }
// it is essential to unsubsribe to the Unity events.
        private void OnDestroy()
        {
            _controller.selectAction.action.started -= Point;
            _controller.selectAction.action.canceled -= PointReleased;
            
            
            _controller.activateAction.action.started -= Fist;
            _controller.activateAction.action.canceled -= FistReleased;
        }

        private void Point(InputAction.CallbackContext ctx)
        {
            _animator.SetBool("Point", true);
            
        }
        private void PointReleased(InputAction.CallbackContext obj)
        {
            _animator.SetBool("Point", false);
        }
        
        private void Fist(InputAction.CallbackContext obj)
        {
            _animator.SetBool("Fist", true);
        }

        private void FistReleased(InputAction.CallbackContext obj)
        {
            _animator.SetBool("Fist", false);
        }



        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
