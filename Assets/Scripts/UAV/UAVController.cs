using System;
using UnityEngine;
using UnityEngine.AI;

namespace UAV
{
    [RequireComponent(typeof(Rigidbody))]
   // [RequireComponent(typeof(NavMeshAgent))]
    public class UAVController : MonoBehaviour
    {
        [Header("UAV Config")]
        [SerializeField] private Transform _rotor1;
        [SerializeField] private Transform _rotor2;
        [SerializeField] private Transform _rotor3;
        [SerializeField] private Transform _rotor4;

        [SerializeField] private float _thrustTakeOff = 98.1f;
        
        [Header("Path Information")]
        [SerializeField] private Transform _targetPose;
        
        private Rigidbody _droneRigidBody;
        private NavMeshAgent _navMeshAgent;
        

        private void Awake()
        {
            if (_droneRigidBody.TryGetComponent<Rigidbody>(out _droneRigidBody))
            {
                _thrustTakeOff = _droneRigidBody.mass * 9.81f;    
            }
            
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //DroneMovement();
            _droneRigidBody.AddRelativeForce(Vector3.up * _thrustTakeOff, ForceMode.Force);
        }

        private void DroneMovement()
        {
            _droneRigidBody.AddForceAtPosition(transform.up * _thrustUp,_rotor1.position, ForceMode.VelocityChange);
            _droneRigidBody.AddForceAtPosition(transform.up * _thrustUp,_rotor2.position, ForceMode.VelocityChange);
            _droneRigidBody.AddForceAtPosition(transform.up * _thrustUp, _rotor3.position * 20, ForceMode.Impulse);
            _droneRigidBody.AddForceAtPosition(transform.up * _thrustUp, _rotor4.position * 20, ForceMode.Impulse);


            // _navMeshAgent.SetDestination(_targetPose.position); //Moves the agent to the target location
        }
    }
}
