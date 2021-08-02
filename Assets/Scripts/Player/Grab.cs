using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform _cameraPostion;
    [SerializeField] private Transform _holdPostion;
    [SerializeField] private float _grabRange = 2f;
    [SerializeField] private float _throwForce = 20f;
    [SerializeField] private float _snapSpeed = 20f;


    private Rigidbody _grabbedObject;
    private bool _grabPressed = false;
    
    //Update works only when all the physics system is completed: When physics is used, use: FixedUpdate()
    void FixedUpdate() 
    {
        if (_grabbedObject)
        {
            _grabbedObject.velocity = (_holdPostion.position - _grabbedObject.position) * _snapSpeed;
        }    
    }

    private void OnGrab()
    {
        if (_grabPressed)
        {
            _grabPressed = false;
            //Debug.Log("Grab Released");
            if(!_grabbedObject) return;
            DropGrabbedObject();
        }
        else
        {
            _grabPressed = true;
            //Debug.Log("Grab pressed");
            if (Physics.Raycast(_cameraPostion.position, _cameraPostion.forward, out RaycastHit hit, _grabRange))
            {
                if (!hit.transform.gameObject.CompareTag("Grabbable")) return;
                _grabbedObject = hit.transform.GetComponent<Rigidbody>();
                _grabbedObject.transform.parent = _holdPostion;
            }
        }
    }

    private void DropGrabbedObject()
    {
        _grabbedObject.transform.parent = null;
        _grabbedObject = null;
    }

    private void OnThrow()
    {
        if (!_grabbedObject) return;
        _grabbedObject.AddForce(_cameraPostion.forward * _throwForce, ForceMode.Impulse);
        StickOnThrow stickObject = _grabbedObject.GetComponentInParent<StickOnThrow>();
        if (stickObject != null)
        {
            stickObject.triggerSticking = true;
        }
        DropGrabbedObject();
       
    }
}
