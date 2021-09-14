using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR
{
    public class SmoothTeleportationAnchor : BaseTeleportationInteractable
    {
        [SerializeField] private float _teleportSpeed = 3f;
        [SerializeField] private float _stoppingDistance = 0.1f;
        
        private Vector3 _teleportEnd;
        private bool _isTeleporting;
        private XRRig _xrRig;
        private XRCustomTeleportationProvider _customTeleportationProvider;
        

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            //Debug.Log("On Select Entered");
            BeginTeleport(args.interactor);
        }

        private void BeginTeleport(XRBaseInteractor interactor)
        {
            _xrRig = interactor.GetComponentInParent<XRRig>();
            _customTeleportationProvider = _xrRig.GetComponent<XRCustomTeleportationProvider>();
            if (_customTeleportationProvider.isTeleporting) return;
            _customTeleportationProvider.TeleportBegin();
            var interactorPos = interactor.transform.localPosition; //TODO: Use head position rather than the hand
            interactorPos.y = 0;
            _teleportEnd = transform.position - interactorPos;
            _isTeleporting = true;

        }

        private void Update()
        {
            
            if (!_isTeleporting) return;
            _xrRig.transform.position = Vector3.MoveTowards(_xrRig.transform.position, _teleportEnd, _teleportSpeed*Time.deltaTime);
            bool distanceCheck = Vector3.Distance(_xrRig.transform.position, _teleportEnd) < _stoppingDistance;
            if (!distanceCheck) return;
            _isTeleporting = false;
            _customTeleportationProvider.TeleportEnd();
        }
    }
}