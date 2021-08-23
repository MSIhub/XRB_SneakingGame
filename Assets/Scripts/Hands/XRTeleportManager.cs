using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
    public class XRTeleportManager : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractor _mainController;
        [SerializeField] private XRBaseInteractor _teleportController;
        [SerializeField] private Animator _handAnimator;
        [SerializeField] private GameObject _pointer;

        private void Start()
        {
            _teleportController.selectEntered.AddListener(MoveSelectedToMainController);
        }

        private void MoveSelectedToMainController(SelectEnterEventArgs arg0)
        {
            
            var interactable = arg0.interactable;

            if (interactable is BaseTeleportationInteractable) return;
            if (interactable)
            {
                _teleportController.interactionManager.ForceSelect(_mainController, interactable);
            }
        }


        // Update is called once per frame
        void Update()
        {
            _pointer.SetActive(_handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Point") && _handAnimator.gameObject.activeSelf);
        }
    }
}
