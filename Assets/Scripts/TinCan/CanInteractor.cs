using UnityEngine;

namespace TinCan
{
    public class CanInteractor : MonoBehaviour
    {
        public bool canFeel = false;
 
        private void OnCollisionEnter(Collision other)
        {
            //Check if the ball hit the can
           // if (!other.gameObject.CompareTag("Grabbable")) return;
        
            //Check if the can feel [use rotation]
            if (transform.localRotation.eulerAngles.x > 5 || transform.localRotation.eulerAngles.z > 5 )
            {
                canFeel = true;
            }
        }
    
    
    }
}
