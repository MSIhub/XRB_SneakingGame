using System;
using UnityEngine;

namespace UAV
{
    public struct DroneState
    {
        public Vector3 Omega { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LinVel { get; set; }
        public Vector3 LinAcc { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 AngVel { get; set; }
        public Vector3 AngAcc { get; set; }
        
    }
    public class UAVRigidBodyDynamics : MonoBehaviour
    {
        [SerializeField] private Transform _drone;
        DroneState drone1;

        private static float R2D = 180f / Mathf.PI;
        private static float D2R = Mathf.PI/180f;
        
        //Input motor 
        [SerializeField] private Vector4 _inputRotorSpeed = new Vector4(1f, 2f,1f,4f) *100f;
        private float _m, _g, _k, _kd, _L, _b;
        private float _Ixx, _Iyy, _Izz;
        private Matrix4x4 Imatrix4;
        private Matrix4x4 Imatrix4_inv;
        
        //LHF to RHF
        private static readonly Matrix4x4 _TLeft2Right = new Matrix4x4(new Vector4(1f, 0f, 0f,0f),
            new Vector4(0f, 0f, 1f,0f), 
            new Vector4(0f, -1f, 0f,0f), 
            new Vector4(0f, 0f, 0f,1f));

        //RHF to LHF
        private static readonly Matrix4x4 _TRight2Left = new Matrix4x4(new Vector4(1f, 0f, 0f,0f), 
            new Vector4(0f, 0f, -1f,0f), 
            new Vector4(0f, 1f, 0f,0f),
            new Vector4(0f, 0f, 0f,1f));
        
        
        private void Start()
        {
            //Initial simulation parameter
            drone1 = new DroneState
            {
                Omega = Vector3.zero,
                Position = _drone.localPosition,
                LinVel = Vector3.zero,
                LinAcc = Vector3.zero,
                Rotation = _drone.parent.localEulerAngles,
                AngVel = Vector3.one*0.05f,
                AngAcc = Vector3.zero
            };

            //Make LHF to RHF
            var (posRhf, rotRhf) = RigidBodyTransformation(_TLeft2Right, drone1.Position, Quaternion.Euler(drone1.Rotation));
            drone1.Position = posRhf;
            drone1.Rotation = rotRhf.eulerAngles;
            //Debug.Log(drone1.Rotation );
            _m = 10f; //Overall Mass of the drone
            _L = 1f; //Length between the rotor blade center [Assuming the same length]
            _Ixx = 10f;
            _Iyy = 10f;
            _Izz = 20f;
            _g = 9.81f; // Acceleration due to gravity
            _k = 0.1f; //Motor lift constant [Measured value specific to a motor]
            _kd = 0.00005f;
            _b = 0.2f;// Drag constant
            
            Imatrix4 = new Matrix4x4(new Vector4(_Ixx, 0f, 0f,0f), 
            new Vector4(0f, _Iyy, 0f,0f), 
            new Vector4(0f, 0f, _Izz,0f),
            new Vector4(0f, 0f, 0f,1f));

            if (_Ixx < 0.001 || _Iyy < 0.001 || _Izz < 0.001 )
            {
                Debug.LogWarning("Inertia Matrix is too low.");
            }
            else
            {
                Imatrix4_inv = new Matrix4x4(new Vector4(1/_Ixx, 0f, 0f,0f), 
                    new Vector4(0f, 1/_Iyy, 0f,0f), 
                    new Vector4(0f, 0f, 1/_Izz,0f),
                    new Vector4(0f, 0f, 0f,1f));
            }


        }

        private void FixedUpdate()
        {
            
            var (posLhf, rotLhf) = RigidBodyDynamicsQuadrotors(drone1);
            _drone.localPosition = posLhf;
            _drone.parent.rotation *= rotLhf;
        }
        private static (Vector3 temp3, Quaternion temp4) RigidBodyTransformation(Matrix4x4 T, Vector3 pos, Quaternion quat)
        {
            Matrix4x4 temp1 = Matrix4x4.Rotate(quat);
            temp1.m03 = pos.x;
            temp1.m13 = pos.y;
            temp1.m23 = pos.z;
            temp1.m33 = 1f;
            
            Matrix4x4 temp2 = T * temp1;
            Vector3 temp3 = new Vector3(temp2.m03, temp2.m13, temp2.m23);
            Quaternion temp4 = QuaternionFromMatrix(temp2);
            return (temp3, temp4);
        }


        private (Vector3 posLHF, Quaternion rotLHF)  RigidBodyDynamicsQuadrotors(DroneState droneState)
        {
            droneState.Omega = AngVel2Omega(droneState.Rotation, droneState.AngVel);
            //Compute acceleration
            droneState.LinAcc = ComputeLinearAcceleration(droneState);
            droneState.AngAcc = ComputeAngularAcceleration(droneState);
           
            //Compute drone state
            droneState.Omega = Time.fixedDeltaTime * droneState.AngAcc; // one shot differentiation
            droneState.AngVel = Omega2AngVel(droneState.Rotation, droneState.Omega);
            droneState.Rotation += Time.fixedDeltaTime * droneState.AngVel;
            
            droneState.LinVel = Time.fixedDeltaTime * droneState.LinAcc;
            droneState.Position += Time.fixedDeltaTime * droneState.LinVel;

            //Make RHF to LHF
            droneState.Rotation = new Vector3(Mathf.Round(droneState.Rotation.x * 100) / 100, Mathf.Round(droneState.Rotation.y * 100) / 100, Mathf.Round(droneState.Rotation.z * 100) / 100);
            var (posLHF, rotLHF) = RigidBodyTransformation(_TRight2Left, droneState.Position, Quaternion.Euler(droneState.Rotation));
            return (posLHF, rotLHF);
        }
        
        private Vector3 AngVel2Omega(Vector3 rotation, Vector3 angVel)
        {
            
            /*
             The angular velocity vector ω != θ. The angular velocity is a vector pointing along the axis of rotation, while thetadot
             is just the time derivative of yaw, pitch, and roll. 
             In order to convert these angular velocities into the angular velocity vector, we can use the following relation
             */
            
            float phi = rotation.x * D2R;
            float theta = rotation.y * D2R;
            float psi = rotation.z * D2R;
            
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
            Vector4 omega = A2O * new Vector4(angVel.x, angVel.y, angVel.z, 0.0f);
            return new Vector3(omega.x * R2D, omega.y * R2D, omega.z* R2D);
        }
        
        private Vector3 ComputeLinearAcceleration(DroneState droneState)
        {
            Vector3 gravity = new Vector3(0.0f, 0.0f, -_g);
            Vector3 thrust = ComputeThrust(_inputRotorSpeed);
            Vector3 thrustRot = RotateZYZEuler_BodyToInertial(droneState.Rotation, thrust);
            Vector3 dragForce = -_kd * droneState.LinVel;
            return gravity + ( (1 / _m) * (thrustRot + dragForce));
        }

        private Vector3 ComputeThrust(Vector4 inputRotorSpeed)
        {
            // Compute thrust given current inputs and thrust coefficient.
            float sumSquareOfInputRotor = (inputRotorSpeed.x * inputRotorSpeed.x) + (inputRotorSpeed.y * inputRotorSpeed.y) + 
                                          (inputRotorSpeed.z * inputRotorSpeed.z) + (inputRotorSpeed.w * inputRotorSpeed.w);
            return new Vector3(0.0f, 0.0f, _k * (sumSquareOfInputRotor));
        }
        
        private Vector3 RotateZYZEuler_BodyToInertial(Vector3 rotation, Vector3 vectorToRotate)
        {
            /*
             We can relate the body and inertial frame by a rotation matrix R which goes from the
            body frame to the inertial frame. This matrix is derived by using the ZYZ Euler angle conventions
            and successively “undoing” the yaw, pitch, and roll.
            For a given vector v in the body frame, the corresponding vector is given by Rv in the inertial frame.
            */
            float phi = rotation.x * D2R;
            float theta = rotation.y * D2R;
            float psi = rotation.z * D2R;
            
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
        private Vector3 ComputeAngularAcceleration(DroneState droneState)
        {
            Vector3 tau = ComputeTorque(_inputRotorSpeed);
            Vector4 temp1 = (Imatrix4 * new Vector4(droneState.Omega.x * D2R, droneState.Omega.y* D2R, droneState.Omega.z* D2R, 0f));
            Vector3 temp2 = (tau - Vector3.Cross(droneState.Omega* D2R, new Vector3(temp1.x, temp1.y, temp1.z)));
            Vector4 temp3 = Imatrix4_inv * new Vector4(temp2.x, temp2.y, temp2.z, 0f);
            return new Vector3(temp3.x* R2D, temp3.y* R2D, temp3.z* R2D);
        }
        
        private Vector3 ComputeTorque(Vector4 inputRotorSpeed)
        {
            float tauX = _L * _k * ((inputRotorSpeed.w * inputRotorSpeed.w) - (inputRotorSpeed.y * inputRotorSpeed.y));
            float tauY = _L * _k * ((inputRotorSpeed.z * inputRotorSpeed.z) -  (inputRotorSpeed.x * inputRotorSpeed.x));
            float tauZ = _b * ((inputRotorSpeed.x * inputRotorSpeed.x) - (inputRotorSpeed.y * inputRotorSpeed.y)
                + (inputRotorSpeed.z * inputRotorSpeed.z) -  (inputRotorSpeed.w * inputRotorSpeed.w));
            return new Vector3(tauX, tauY, tauZ);
        }

        private Vector3 Omega2AngVel(Vector3 rotation, Vector3 omega)
        {
            float phi = rotation.x* D2R;
            float theta = rotation.y* D2R;
            float psi = rotation.z* D2R;
            
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
            A2O[3, 3] = 1f;
            //Debug.Log(A2O);
            Vector4 angVel = A2O.inverse * new Vector4(omega.x, omega.y, omega.z, 0.0f);
            return new Vector3(angVel.x* R2D, angVel.y* R2D, angVel.z* R2D);
        }
        
        public static Quaternion QuaternionFromMatrix(Matrix4x4 m) {
            // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
            Quaternion q = new Quaternion();
            q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] + m[1,1] + m[2,2] ) ) / 2; 
            q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] - m[1,1] - m[2,2] ) ) / 2; 
            q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] + m[1,1] - m[2,2] ) ) / 2; 
            q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] - m[1,1] + m[2,2] ) ) / 2; 
            q.x *= Mathf.Sign( q.x * ( m[2,1] - m[1,2] ) );
            q.y *= Mathf.Sign( q.y * ( m[0,2] - m[2,0] ) );
            q.z *= Mathf.Sign( q.z * ( m[1,0] - m[0,1] ) );
            return q;
        }

        /*[00 01 02 03
        10 11 12 13
        20 21 22 23
        30 31 32 33]*/
        
        



    }
}