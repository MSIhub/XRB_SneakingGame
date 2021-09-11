using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UAV
{
    public class PIDTest : MonoBehaviour
    {

        [SerializeField] private float _Kp = 0.1f;
        [SerializeField] private float _Ki = 0.1f;
        [SerializeField] private float _Kd = 0.1f;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _targetPose;
    
        private Vector3 _error;
        private Vector3 _setPoint;
        private Vector3 _proportionalTerm;
        private Vector3 _integralTerm;
        private Vector3 _derivativeTerm;
        private Vector3 _diffProcessVariable = Vector3.zero;
        private Vector3 _processVariable =Vector3.zero;

        private IEnumerator motionCoroutine;
        private Vector3 ProcessVariable
        {
            get => _processVariable;
            set
            {
                ProcessVariableLast = _processVariable;
                _processVariable = value;
            }
        }

        private Vector3 ProcessVariableLast { get; set; } = Vector3.zero;
    
        // Update is called once per frame
        private void FixedUpdate()
        { 
            _setPoint = _targetPose.position;//set point
            ProcessVariable = _rb.position; //process variable
            _diffProcessVariable = (ProcessVariable) - (ProcessVariableLast);// derivative error
            _error = _setPoint - ProcessVariable; //error
            //if ((_error.magnitude < 0.5f)) 
            if (motionCoroutine != null)
            {
                StopCoroutine(motionCoroutine);
            }
            motionCoroutine = DoMotion();
            StartCoroutine(motionCoroutine);

        }


        private IEnumerator DoMotion()
        {
            while (_error.magnitude > 0.5f)
            {
                _integralTerm += (_Ki * _error * Time.fixedDeltaTime);// integral term calculation
                //Vector3.ClampMagnitude()
                _derivativeTerm = _Kd * (_diffProcessVariable / Time.fixedDeltaTime);// derivative term calculation
                _proportionalTerm = (_Kp * _error); // Proportional term calculation
                var pidOutput = _proportionalTerm + _integralTerm - _derivativeTerm;
                _rb.AddForce(pidOutput, ForceMode.Impulse);
                yield return null;    
            }
        }
    

    }
}


/*        [Button]
        private void DoMotion()
        {
            if ((_error.magnitude < 0.5f)) return;
            _integralTerm += (_Ki * _error * Time.fixedDeltaTime);// integral term calculation
            //Vector3.ClampMagnitude()
            _derivativeTerm = _Kd * (_diffProcessVariable / Time.fixedDeltaTime);// derivative term calculation
            _proportionalTerm = (_Kp * _error); // Proportional term calculation
            var pidOutput = _proportionalTerm + _integralTerm - _derivativeTerm;
            _rb.AddForce(pidOutput, ForceMode.Impulse);
        }*/