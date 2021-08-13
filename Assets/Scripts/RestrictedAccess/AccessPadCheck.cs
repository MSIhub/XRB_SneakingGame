using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class AccessPadCheck : MonoBehaviour
{
    [SerializeField] private string _uniqueIdentifier = "xrbt1";
    [SerializeField] private GameObject _door;

    [InfoBox("Below values are configured for the mesh of the access pad and key card")]
    [ShowInInspector, ReadOnly] private float _distanceToCheck = 0.2f;
    [ShowInInspector, ReadOnly] private float _sphereCastRadius = 0.2f;
    
    private KeyCardHolder _keyCardHolder;
    private void FixedUpdate()
    {
       
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, _sphereCastRadius,transform.TransformDirection(Vector3.up), out hit, _distanceToCheck))
        {
            Debug.Log("Did Hit"+ hit.transform.name);
            if (hit.transform.gameObject.TryGetComponent<KeyCardHolder>(out _keyCardHolder))
            {
                if (_uniqueIdentifier == _keyCardHolder.uniqueIdentifierKeyCard)
                { 
                    Debug.Log("Yay, found the right key!");
                    AttachKey();
                    //keyCardHolder.gameObject.transform.position = transform.position;
                }
            }
        }
    }

    private void AttachKey()
    {
        Transform KeyCard = _keyCardHolder.transform; 
        if (KeyCard.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        
        KeyCard.parent = transform;
        KeyCard.localPosition = new Vector3(-0.105f, -0.01f, 0.025f);  //based on the geometry of the key card and the access holder
        KeyCard.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
        OpenDoor();
    }


    private void OpenDoor()
    {
        DoorTrigger doorTrigger = _door.GetComponentInChildren<DoorTrigger>();
        if (doorTrigger != null)
        {
            doorTrigger.gameObject.SetActive(false);
        }
        
        
        DoorMesh doorMesh = _door.GetComponentInChildren<DoorMesh>();
        if (doorMesh != null)
        {
            doorMesh.gameObject.SetActive(false);    
        }


        
    }

}
