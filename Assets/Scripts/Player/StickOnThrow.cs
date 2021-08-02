using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StickOnThrow : MonoBehaviour
{
    public bool triggerSticking = false;
    private void OnCollisionEnter(Collision other)
    {
        if (triggerSticking)
        {
            transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;//removes the physics
        }
    }
}
