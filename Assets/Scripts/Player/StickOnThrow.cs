using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StickOnThrow : MonoBehaviour
{
    public bool triggerSticking = false;

    [SerializeField] private float _bombRange = 5.0f;
    private bool _stuckToWall = false;

    private void FixedUpdate()
    {
        if (_stuckToWall)
        {
            if (Physics.Raycast(transform.position, Vector3.forward, _bombRange))
            {
                Debug.Log("detected object");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        StickToCollidedSurfaceOnThrow();
    }

    private void StickToCollidedSurfaceOnThrow()
    {
        if (triggerSticking)
        {
            transform.gameObject.GetComponent<Rigidbody>().isKinematic = true; //removes the physics
            _stuckToWall = true;
        }
    }
}
