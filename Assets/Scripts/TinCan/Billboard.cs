using UnityEngine;

namespace TinCan
{
    public class Billboard : MonoBehaviour
    {

        [SerializeField] private Transform cam;
    
        void LateUpdate()//After the update
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}
