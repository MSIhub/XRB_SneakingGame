using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace DefaultNamespace
{
    public class ExplodeRobot : MonoBehaviour
    {
        [SerializeField] private GameObject _destroyedGameObject;
        //[SerializeField] private float _explodeForceIntensity = 10.0f;
        

        private void Start()
        {
            //ActivateRobotExplosion();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Grabbable")) return;
            ActivateRobotExplosion();
        }

        public void ActivateRobotExplosion()
        {
            GameObject go = Instantiate(_destroyedGameObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}