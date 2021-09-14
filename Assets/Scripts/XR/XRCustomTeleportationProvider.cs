using UnityEngine;

namespace XR
{
    public class XRCustomTeleportationProvider : MonoBehaviour
    {
        [SerializeField] private Animator _vignetteAnimator;
        public bool isTeleporting;

        public void TeleportBegin()
        {
            isTeleporting = true;
            _vignetteAnimator.SetBool("isTeleporting", isTeleporting);
        }

        public void TeleportEnd()
        {
            isTeleporting = false;
            _vignetteAnimator.SetBool("isTeleporting", isTeleporting);
        }
    }
}
