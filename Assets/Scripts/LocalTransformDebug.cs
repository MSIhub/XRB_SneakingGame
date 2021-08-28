using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LocalTransformDebug : MonoBehaviour
    {
        [SerializeField] private Vector3 Postion;
        [SerializeField] private Vector3 localRotation;

        private void Update()
        {
            /*Vector3 incPos = transform.worldToLocalMatrix * new Vector3(0f, 0.001f, 0f);
            transform.localPosition += incPos;*/
            transform.localPosition += new Vector3(0f, 0.001f, 0f);
            Postion = transform.localPosition;
            localRotation = transform.localRotation.eulerAngles;
            
            
        }
    }
}