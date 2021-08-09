using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ExplodeRobot : MonoBehaviour
    {
        [SerializeField] private Transform _objectToExplode;
        [SerializeField] private float _ExplodeForceIntensity;
        private Rigidbody _cloneObject;
        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
              _cloneObject = Instantiate(_objectToExplode, transform.position + new Vector3(0, i* 1.0f, 0), Quaternion.identity, transform.parent).GetComponent<Rigidbody>();
              _cloneObject.AddForce(_ExplodeForceIntensity*_cloneObject.transform.up, ForceMode.Impulse);
            }
        }

    }
}