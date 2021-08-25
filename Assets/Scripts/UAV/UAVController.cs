using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace UAV
{
    [RequireComponent(typeof(Rigidbody))]
    //[RequireComponent(typeof(NavMeshAgent))]
    public class UAVController : MonoBehaviour
    {
        [Header("UAV Config")] 
        [SerializeField] private Transform _rotor1;
        [SerializeField] private Transform _rotor2;
        [SerializeField] private Transform _rotor3;
        [SerializeField] private Transform _rotor4;
        //TODO: Get rotor in a list if its not a quadrotor
        
        [Header("Input")] 
        [SerializeField] private float _thrustTakeOff = 98.1f;
        [SerializeField] private float _thrustMove = 10.0f;

        [Header("Target")]
        [SerializeField] private Transform _targetPose;

        private Rigidbody _droneRigidBody;
        private NavMeshPath _pathNavMesh;
        private float _elapsed = 0.0f;
      //  private int _index = 0;
        private Vector3 _forceDir = Vector3.zero;
        

        private void Start()
        {
            if (gameObject.TryGetComponent<Rigidbody>(out _droneRigidBody))
            {
                _thrustTakeOff = _droneRigidBody.mass * 9.81f;
            }

            _pathNavMesh = new NavMeshPath();
            _elapsed = 0.0f;

        }

        void Update()
        {
            // Update the way to the goal every second.
            _elapsed += Time.deltaTime;
            if (_elapsed > 1.0f)
            {
                _elapsed -= 1.0f;
                NavMesh.CalculatePath(transform.position, _targetPose.position, NavMesh.AllAreas, _pathNavMesh);
            }

            for (int i = 0; i < _pathNavMesh.corners.Length - 1; i++)
            {
                Debug.DrawLine(_pathNavMesh.corners[i], _pathNavMesh.corners[i + 1], Color.red);
            }

            if (_pathNavMesh.status != NavMeshPathStatus.PathComplete) return;
            _forceDir = (_pathNavMesh.corners[1] - _pathNavMesh.corners[0]).normalized;
            _droneRigidBody.AddRelativeForce(_forceDir * _thrustMove, ForceMode.Impulse);

           // RigidBodyDynamicsQuadrotors();
        }


    }
}   
