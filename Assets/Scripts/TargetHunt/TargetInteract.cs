using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace TargetHunt
{
    public class TargetInteract : MonoBehaviour
    {
        //[SerializeField] private ResetMeshInteractor _resetMesh;
        public bool isHit = false;
        private Vector3 _offsetTargetDown;
        private int _count;
       // private int _countUp;

        // Start is called before the first frame update
        void Start()
        {
            _count = 0;
       //     _countUp = 0;
            _offsetTargetDown = new Vector3(0f, 0.02f, 0f);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<BulletInteractor>(out BulletInteractor bullet))
            {
                _count++;
                isHit = true;
                TargetDown();
            }
            
        }
        private void TargetDown()
        {
            if (_count == 1 && isHit)
            {
                var lockPosition = transform.position;
                transform.position = lockPosition + _offsetTargetDown;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f); 
            }
        }

        /*private void TargetUp()
        {
            _countUp++;
            if (_countUp == 1 && isHit)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f); 
                var lockPosition = transform.position;
                transform.position = lockPosition - _offsetTargetDown;
                isHit = false;
            }
        }*/

        
        
    }
}
