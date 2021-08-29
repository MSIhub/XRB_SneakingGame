using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
    public class XRButtonPress : MonoBehaviour
    {
        [SerializeField] private Transform _buttonMesh;
        [SerializeField] private Camera _minimapCamera;
        private bool _isButtonPressed = false;//Course object at the start makes it true
        private void OnTriggerEnter(Collider other)
        {


            if (other.gameObject.GetComponent<XRBaseControllerInteractor>()) return;
            if (!_isButtonPressed)
            {
            
                _buttonMesh.localScale -= new Vector3(0f, 1f, 0.0f);
                _buttonMesh.localPosition -= new Vector3(0f, 0.01f, 0.0f);
                _isButtonPressed = true;    
                if (gameObject.TryGetComponent<CapsuleCollider>(out var buttonCollider))
                {
                    buttonCollider.center -= new Vector3(0f, 0.02f, 0.0f);
                }
                
                _minimapCamera.cullingMask |= 1 << LayerMask.NameToLayer("Water");//add water layer to culling mask

            }
            else
            {
                _buttonMesh.localScale += new Vector3(0f, 1f, 0.0f);
                _buttonMesh.localPosition += new Vector3(0f, 0.01f, 0.0f);
                _isButtonPressed = false;   
                if (gameObject.TryGetComponent<CapsuleCollider>(out var buttonCollider))
                {
                    buttonCollider.center += new Vector3(0f, 0.02f, 0.0f);
                }
                _minimapCamera.cullingMask &=  ~(1 << LayerMask.NameToLayer("Water"));//remove water layer to culling mask
                
               
            }
            
        }
    }
}
