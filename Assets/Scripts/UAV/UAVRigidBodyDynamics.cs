using System;
using UnityEngine;

namespace UAV
{
    public class UAVRigidBodyDynamics : MonoBehaviour
    {
        [SerializeField] private Transform _drone;
        
        //Initial simulation parameter
        private Vector3 _position = Vector3.zero; //meters
        private Vector3 _linVel = Vector3.zero; //meters per second
        private Vector3 _linAcc = Vector3.zero; //radians per second [Initialize some disturbances]
        
        private Vector3 _rotation = Vector3.zero; //radians
        private Vector3 _angVel = Vector3.zero; //radians per second [Initialize some disturbances]
        private Vector3 _omega = Vector3.zero; //radians per second [Initialize some disturbances]
        private Vector3 _angAcc = Vector3.zero; //radians per second [Initialize some disturbances]
        
        //Input motor 
        private Vector4 _inputRotorSpeed = Vector4.zero;
        private float _m, _g, _k, _kd, _L, _b;
        private float _Ixx, _Iyy, _Izz;
        
        //LHF to RHF
        private Matrix4x4 _TLeft2Right = new Matrix4x4(new Vector4(1f, 0f, 0f,0f), new Vector4(0f, 0f, 1f,0f), new Vector4(0f, -1f, 0f,0f), new Vector4(0f, 0f, 0f,1f));
        //RHF to LHF
        private Matrix4x4 _TRight2Left = new Matrix4x4(new Vector4(1f, 0f, 0f,0f), new Vector4(0f, 0f, -1f,0f), new Vector4(0f, 1f, 0f,0f), new Vector4(0f, 0f, 0f,1f));
        
        
        private void Start()
        {
            _position = _drone.position;
            _rotation = _drone.eulerAngles;
            _m = 1.25f;
            _L = 0.265f;
            _Ixx = 0.0232f;
            _Iyy = 0.0232f;
            _Izz = 0.0468f;
            _g = 9.81f;
            _k = 0.001f;
            _kd = 0.005f;
            _b = 0.001f;

        }

        private void Update()
        {
            //RigidBodyDynamicsQuadrotors();
           //Make RHF to LHF
            
            
            
            _drone.position = new Vector3(_position.x, -_position.z, _position.y);
            Debug.Log(_drone.position);
            
        }


        private void RigidBodyDynamicsQuadrotors()
        {
            _omega = AngVel2Omega(_rotation, _angVel);

            //Compute acceleration
            _linAcc = ComputeLinearAcceleration();
            _angAcc = ComputeAngularAcceleration();
            
            //Compute drone state
            _omega += Time.fixedDeltaTime * _angAcc; // one shot differentiation
            _angVel = Omega2AngVel(_rotation, _omega);
            _rotation += Time.fixedDeltaTime * _angVel;
            _linVel += Time.fixedDeltaTime * _linAcc;
            _position += Time.fixedDeltaTime * _linVel;
        }


        private Vector3 ComputeAngularAcceleration()
        {
            Vector3 tau = ComputeTorque();
            Matrix4x4 I = ComputeInertiaMatrix();
            Vector4 temp1 = (I * new Vector4(_omega.x, _omega.y, _omega.z, 0f));
            Vector3 temp2 = (tau - Vector3.Cross(_omega, new Vector3(temp1.x, temp1.y, temp1.z)));
            Vector4 temp3 = I.inverse * new Vector4(temp2.x, temp2.y, temp2.z, 0f);
            return new Vector3(temp3.x, temp3.y, temp3.z);
        }

        private Vector3 ComputeLinearAcceleration()
        {
            Vector3 gravity = new Vector3(0.0f, 0.0f, -_g);
            Vector3 thrust = ComputeThrust();
            Vector3 thrustRot = RotateZYZEuler_BodyToInertial(_rotation, thrust);
            Vector3 dragForce = -_kd * _linVel;
            return gravity + ( (1 / _m) * (thrustRot + dragForce));
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
            Vector4 omega = A2O * new Vector4(angVel.x, angVel.y, angVel.z, 0.0f);
            return new Vector3(omega.x, omega.y, omega.z);
        }
        
        private Vector3 Omega2AngVel(Vector3 rotation, Vector3 omega)
        {
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
            Vector4 angVel = A2O.inverse * new Vector4(omega.x, omega.y, omega.z, 0.0f);
            
            return new Vector3(angVel.x, angVel.y, angVel.z);
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
            
            Matrix4x4 R = Matrix4x4.zero;
            R[0, 0] = Mathf.Cos(phi)* Mathf.Cos(psi) - (Mathf.Cos(theta) * Mathf.Sin(phi)* Mathf.Sin(psi));
            R[0, 1] = -(Mathf.Cos(psi)* Mathf.Sin(phi)) - (Mathf.Cos(phi) * Mathf.Cos(theta)* Mathf.Sin(psi));
            R[0, 2] = Mathf.Sin(theta) * Mathf.Sin(psi);
            R[1, 0] = (Mathf.Cos(theta) * Mathf.Cos(psi) * Mathf.Sin(phi)) + (Mathf.Cos(phi)* Mathf.Sin(psi));
            R[1, 1] = (Mathf.Cos(phi) * Mathf.Cos(theta) * Mathf.Cos(psi)) + (Mathf.Sin(phi)* Mathf.Sin(psi));
            R[1, 2] = -(Mathf.Cos(psi) * Mathf.Sin(theta));
            R[2, 0] = Mathf.Sin(phi) * Mathf.Sin(theta);
            R[2, 1] = -Mathf.Cos(psi) * Mathf.Sin(theta);
            R[2, 2] = Mathf.Cos(theta);
            
            Vector4 rotatedVector = R * new Vector4(vectorToRotate.x, vectorToRotate.y, vectorToRotate.z, 0.0f);
            return new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z);
        }
        
        private Vector3 ComputeThrust()
        {
            // Compute thrust given current inputs and thrust coefficient.
            float sumOfInputRotor = _inputRotorSpeed.x + _inputRotorSpeed.y + _inputRotorSpeed.z + _inputRotorSpeed.w;
            return new Vector3(0.0f, 0.0f, _k * (sumOfInputRotor));
        }
        
        private Vector3 ComputeTorque()
        {
            float tauX = _L * _k * (_inputRotorSpeed.x - _inputRotorSpeed.z);
            float tauY = _L * _k * (_inputRotorSpeed.y - _inputRotorSpeed.w);
            float tauZ = _b * (_inputRotorSpeed.x - _inputRotorSpeed.y + _inputRotorSpeed.z - _inputRotorSpeed.w);
            return new Vector3(tauX, tauY, tauZ);
        }
        
        private Matrix4x4 ComputeInertiaMatrix()
        { 
            /*
            We can model our quadcopter as two thin uniform rods crossed at the origin with a point mass
            (motor) at the end of each. With this in mind, it’s clear that the symmetries result in a diagonal
            inertia matrix of the form I
            */
            Matrix4x4 I = Matrix4x4.zero;
            I[0,0] = _Ixx;
            I[1,1] = _Iyy;
            I[2,2] = _Izz;
            return I;
        }


    }
}