using UnityEngine;


public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit");
        }
        else
        {
            Debug.Log("Hit Not obstacle");
        }
    }
}
