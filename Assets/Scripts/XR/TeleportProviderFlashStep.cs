using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Hands;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR
{
    public class TeleportProviderFlashStep : TeleportationProvider
    {
        [SerializeField] private bool _isTranslate = true;
        [SerializeField] private Canvas _fadeOut;
        private float _lerpValue = 0.0f;
        private Coroutine _translateTeleport;

        private void Start()
        {
            _fadeOut.gameObject.SetActive(false);
        }

        protected override void Update()
       {
           if (!validRequest || !BeginLocomotion())
               return;

           var xrRig = system.xrRig;
           if (xrRig != null)
           {
               switch (currentRequest.matchOrientation)
               {
                   case MatchOrientation.WorldSpaceUp:
                       xrRig.MatchRigUp(Vector3.up);
                       break;
                   case MatchOrientation.TargetUp:
                       xrRig.MatchRigUp(currentRequest.destinationRotation * Vector3.up);
                       break;
                   case MatchOrientation.TargetUpAndForward:
                       xrRig.MatchRigUpCameraForward(currentRequest.destinationRotation * Vector3.up, currentRequest.destinationRotation * Vector3.forward);
                       break;
                   case MatchOrientation.None:
                       // Change nothing. Maintain current rig rotation.
                       break;
                   default:
                       Assert.IsTrue(false, $"Unhandled {nameof(MatchOrientation)}={currentRequest.matchOrientation}.");
                       break;
               }

               if (_isTranslate)
               {
                   _fadeOut.gameObject.SetActive(true);
                   var cameraDestination = currentRequest.destinationPosition;
                   if (_translateTeleport != null)
                   {
                       StopCoroutine(_translateTeleport);
                   }
                   _translateTeleport = StartCoroutine(TranslateTeleportCoroutine(xrRig, cameraDestination));
               }
               else
               {
                   var heightAdjustment = xrRig.rig.transform.up * xrRig.cameraInRigSpaceHeight;
                   var cameraDestination = currentRequest.destinationPosition + heightAdjustment;
                   xrRig.MoveCameraToWorldLocation(cameraDestination);
               }
           }
           EndLocomotion();
           validRequest = false;
       }

        private IEnumerator TranslateTeleportCoroutine(XRRig rig, Vector3 dest)
       {
           var position = rig.transform.position;
           var destination = dest;
           
           if (Vector3.Distance(dest, rig.transform.position) > 0.5f)
           {
               _lerpValue = 0f;

           }
           
           while (_lerpValue <= 1f)
           {
               _lerpValue += 0.1f;
               var xlerp = Mathf.Lerp(position.x, destination.x, _lerpValue);
               var ylerp = Mathf.Lerp(position.z, destination.z, _lerpValue);
               var nextPosition = new Vector3(xlerp, position.y, ylerp);
               rig.transform.position = (nextPosition);
               yield return null;
           }
       }
       
    }
}
