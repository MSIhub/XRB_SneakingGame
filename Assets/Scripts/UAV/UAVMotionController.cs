using Sirenix.OdinInspector;
using UnityEngine;

namespace UAV
{
    public class UAVMotionController : MonoBehaviour
    {

        [SerializeField] private Vector3 _positionDisplacementVec;
        [SerializeField] private Vector3 _orientationDisplacementVec;

        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private Matrix4x4 _transformationMat;

        private Matrix4x4 _transformationMatDisp;
        // Start is called before the first frame update
        void Start()
        {
            _positionDisplacementVec = Vector3.zero;
            _orientationDisplacementVec = Vector3.zero;
            _startPosition = transform.position;
            _startRotation = transform.rotation;

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = _startPosition + (_positionDisplacementVec.y * transform.up);
            Debug.DrawRay(transform.position, transform.up*0.4f, Color.red);
            Debug.DrawRay(transform.position, transform.right*0.4f, Color.red);

        }
        [Button] 
        private void RotateDrone()
        {
            Vector3 direction = transform.position - transform.right;
            Debug.Log(transform.right);
            Quaternion newRotation = Quaternion.AngleAxis(_orientationDisplacementVec.x, (transform.right)) *  Quaternion.LookRotation(direction);;
            //transform.Rotate(_orientationDisplacementVec);
            transform.rotation = newRotation;
        }
    }
}


/*void Update()
{
    Vector3 _nextPosition = _startPosition + (_positionDisplacementVec.y * transform.up);
    _transformationMat = Matrix4x4.TRS(_startPosition, transform.rotation, transform.localScale);
    _transformationMatDisp = Matrix4x4.TRS(_nextPosition, Quaternion.Euler(_orientationDisplacementVec), transform.localScale);
    //Debug.Log(TransformationMat);

            
    //Debug.DrawLine(transform.position, _nextPosition, Color.magenta);
    Debug.DrawRay(transform.position, transform.up*0.2f, Color.red);

}
[Button]
private void Transform()
{
    Matrix4x4 newTransMat = (_transformationMat * _transformationMatDisp);
    transform.position = (newTransMat.GetColumn(3));
    gameObject.transform.rotation = (newTransMat.rotation);
}
}*/