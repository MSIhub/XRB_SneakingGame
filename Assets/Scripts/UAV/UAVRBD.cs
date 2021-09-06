using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UAV
{
    [RequireComponent(typeof(Rigidbody))]
    public class UAVRBD : MonoBehaviour
    {
        private float _L = 1f; //Length between the rotor blade center [Assuming the same length]
        private float _k = 0.1f; //Motor lift constant [Measured value specific to a motor]
        private float _b = 0.2f;// Drag constant
        private Rigidbody rb;
        [SerializeField] private Vector3 _thrust =new Vector3(0.0f,1.0f,0.0f);
        [SerializeField] private Vector3 _torque =new Vector3(1.0f,0.0f,0.0f);
        [SerializeField] private Vector4 _inputRotorSpeed = new Vector4(1f, 1f,1f,1f) *1f;
        //RHF to LHF
        private static readonly Matrix4x4 _TRight2Left = new Matrix4x4(new Vector4(1f, 0f, 0f,0f), 
            new Vector4(0f, 0f, -1f,0f), 
            new Vector4(0f, 1f, 0f,0f),
            new Vector4(0f, 0f, 0f,1f));

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 thrustRhf = ComputeThrust(_inputRotorSpeed);
            Vector3 thrustLhf = _TRight2Left* thrustRhf;
            rb.AddForce(_thrust,ForceMode.Acceleration);
            //rb.AddForceAtPosition(thrustLhf,transform.position,ForceMode.Acceleration);

           // Vector3 torqueRhf = ComputeTorque(_inputRotorSpeed);
           // Vector3 torqueLhf = _TRight2Left* torqueRhf;
            //rb.AddRelativeTorque(new Vector3(1.0f,0.0f,0.0f),ForceMode.VelocityChange);
            
            
        }
        [Button]
        private void AddTorque()
        {
            rb.AddTorque(_torque, ForceMode.Acceleration);
        }

        private Vector3 ComputeThrust(Vector4 inputRotorSpeed)
        {
            // Compute thrust given current inputs and thrust coefficient.
            float sumSquareOfInputRotor = (inputRotorSpeed.x * inputRotorSpeed.x) + (inputRotorSpeed.y * inputRotorSpeed.y) + 
                                          (inputRotorSpeed.z * inputRotorSpeed.z) + (inputRotorSpeed.w * inputRotorSpeed.w);
            return new Vector3(0.0f, 0.0f, _k * (sumSquareOfInputRotor));
        }
        
        private Vector3 ComputeTorque(Vector4 inputRotorSpeed)
        {
            float tauX = _L * _k * ((inputRotorSpeed.w * inputRotorSpeed.w) - (inputRotorSpeed.y * inputRotorSpeed.y));
            float tauY = _L * _k * ((inputRotorSpeed.z * inputRotorSpeed.z) -  (inputRotorSpeed.x * inputRotorSpeed.x));
            float tauZ = _b * ((inputRotorSpeed.x * inputRotorSpeed.x) - (inputRotorSpeed.y * inputRotorSpeed.y)
                + (inputRotorSpeed.z * inputRotorSpeed.z) -  (inputRotorSpeed.w * inputRotorSpeed.w));
            return new Vector3(tauX, tauY, tauZ);
        }

    }
}