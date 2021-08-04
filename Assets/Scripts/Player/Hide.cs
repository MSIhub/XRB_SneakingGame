using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    //measured according to the mesh
    [SerializeField] private Transform _cameraPostion;
    [SerializeField] private Transform _cloakAnchor;
    [SerializeField] private float _hideObjectRange = 2f;

    private GameObject _cloakObject;
    private bool _hidePressed = false;
    
    
    void Update() 
    {
        if (_cloakObject)
        {
            _cloakObject.transform.position = _cloakAnchor.position;
            _cloakObject.transform.rotation = _cloakAnchor.rotation;
        }
 
    }
    
    private void OnHide() 
    {
        if (_hidePressed)
        {
            _hidePressed = false;
            if(!_cloakObject) return;
            DropHideObject();
        }
        else
        {
            _hidePressed = true;
            if (Physics.Raycast(_cameraPostion.position, _cameraPostion.forward, out RaycastHit hit, _hideObjectRange))
            {
                if (!hit.transform.gameObject.CompareTag("Cloak")) return;
                _cloakObject = hit.transform.gameObject;
                _cloakObject.transform.parent = _cloakAnchor;
            }
        }
    }
    
    private void DropHideObject()
    {
        _cloakObject.transform.parent = null;
        _cloakObject = null;
    }

}
