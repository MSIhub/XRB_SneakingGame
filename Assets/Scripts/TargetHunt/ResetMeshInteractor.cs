using UnityEngine;
using UnityEngine.SceneManagement;

namespace TargetHunt
{
    
    public class ResetMeshInteractor : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<BulletInteractor>(out var bullet))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
