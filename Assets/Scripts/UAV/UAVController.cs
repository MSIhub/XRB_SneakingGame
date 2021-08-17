using System;
using UnityEngine;
using UnityEngine.AI;

namespace UAV
{
    [RequireComponent(typeof(Rigidbody))]
    //[RequireComponent(typeof(NavMeshAgent))]
    public class UAVController : MonoBehaviour
    {
        [Header("UAV Config")] [SerializeField]
        private Transform _rotor1;

        [SerializeField] private Transform _rotor2;
        [SerializeField] private Transform _rotor3;

        [SerializeField] private Transform _rotor4;
        //TODO: Get rotor in a list

        [Header("Input")] [SerializeField] private float _thrustTakeOff = 98.1f;
        [SerializeField] private float _thrustMove = 10.0f;

        [Header("Target")] [SerializeField] private Transform _targetPose;

        private Rigidbody _droneRigidBody;
        private NavMeshPath _pathNavMesh;
        private float _elapsed = 0.0f;
        private int _index = 0;
        private Vector3 _forceDir = Vector3.zero;

        //Initial simulation time
        private Vector3 _position = Vector3.zero; //meters
        private Vector3 _velocity = Vector3.zero; //meters per second
        private Vector3 _rotation = Vector3.zero; //radians
        private Vector3 _angVel = Vector3.zero; //radians per second [Initialize some disturbances]
        private Vector3 _omega = Vector3.zero; //radians per second [Initialize some disturbances]
        private Vector4 _inputRotorSpeed = Vector4.zero;





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

            RigidBodyDynamics_Quadrotors();
        }

        private void DroneMovement()
        {
            /*_droneRigidBody.AddForceAtPosition(transform.up * _thrustUp,_rotor1.position, ForceMode.VelocityChange);
            _droneRigidBody.AddForceAtPosition(transform.up * _thrustUp,_rotor2.position, ForceMode.VelocityChange);
            _droneRigidBody.AddForceAtPosition(transform.up * _thrustUp, _rotor3.position * 20, ForceMode.Impulse);
            _droneRigidBody.AddForceAtPosition(transform.up * _thrustUp, _rotor4.position * 20, ForceMode.Impulse);*/


            // _navMeshAgent.SetDestination(_targetPose.position); //Moves the agent to the target location
        }


        private void RigidBodyDynamics_Quadrotors()
        {
            //_angVel = new Vector3(10.2f, 1f, 5f);
            //_rotation = new Vector3(0f, 1.25f, 0f);
            _omega = AngVel2Omega(_rotation, _angVel);
            Debug.Log(_omega);

        }

        private Vector3 AngVel2Omega(Vector3 rotation, Vector3 angVel)
        {
            
            /*
             The angular velocity vector ω != θ. The angular velocity is a vector pointing along the axis of rotation, while thetadot
             is just the time derivative of yaw, pitch, and roll. 
             In order to convert these angular velocities into the angular velocity vector, we can use the following relation
             */
            
            float phi = rotation.x;
            float theta = rotation.y;
            float psi = rotation.z;
            
            //Instead of creating 3*3 matrix, I used the 4*4 with zero additional
            Matrix4x4 A2O = Matrix4x4.zero;
            A2O[0, 0] = 1f;
            A2O[0, 1] = 0f;
            A2O[0, 2] = -Mathf.Sin(theta);
            A2O[1, 0] = 0f;
            A2O[1, 1] = Mathf.Cos(phi);
            A2O[1, 2] = Mathf.Cos(theta) * Mathf.Sin(phi);
            A2O[2, 0] = 0f;
            A2O[2, 1] = -Mathf.Sin(phi);
            A2O[2, 2] = Mathf.Cos(theta) * Mathf.Cos(phi);
            //Debug.Log(A2O);
            Vector3 omega = A2O * new Vector4(angVel.x, angVel.y, angVel.z, 0.0f);
            return omega;
        }

        /*[00 01 02 03
        10 11 12 13
        20 21 22 23
        30 31 32 33]*/
        private Vector3 RotateZYZEuler_BodyToInertial(Vector3 rotation, Vector3 vectorToRotate)
        {
            /*
             We can relate the body and inertial frame by a rotation matrix R which goes from the
            body frame to the inertial frame. This matrix is derived by using the ZYZ Euler angle conventions
            and successively “undoing” the yaw, pitch, and roll.
            For a given vector v in the body frame, the corresponding vector is given by Rv in the inertial frame.
            */

            float phi = rotation.x;
            float theta = rotation.y;
            float psi = rotation.z;
            
            Vector3 rotatedVector = Vector3.zero;
            
            Matrix4x4 R = Matrix4x4.zero;
            R[0, 0] = 1f;
            R[0, 1] = 0f;
            R[0, 2] = -Mathf.Sin(theta);
            R[1, 0] = 0f;
            R[1, 1] = Mathf.Cos(phi);
            R[1, 2] = Mathf.Cos(theta) * Mathf.Sin(phi);
            R[2, 0] = 0f;
            R[2, 1] = -Mathf.Sin(phi);
            R[2, 2] = Mathf.Cos(theta) * Mathf.Cos(phi);

            return rotatedVector;
        }
    }
}   
