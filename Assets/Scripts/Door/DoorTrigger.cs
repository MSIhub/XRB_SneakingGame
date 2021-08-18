using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            OpenDoor();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DoorInteractor>())
        {
            CloseDoor();
        }
    }

    protected void OpenDoor()
    {
        door.SetActive(false);   
    }
    protected void CloseDoor()
    {
        door.SetActive(true); 
    }
}
