using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            _door.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            _door.SetActive(true);
        }
    }
}
