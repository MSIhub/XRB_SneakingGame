using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    [SerializeField] private CharacterController _charController; // [serializefield] will expose a private variable in the editor
    [SerializeField] private float _crouchHeight = 1;
    private float _originalHeight;
    private bool _crouched = false;

    // Start is called before the first frame update
    void Start()
    {
        _originalHeight = _charController.height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCrouch() //name of the function is predefined for an action
    {
        if (_crouched)
        {
            _crouched = false;
            _charController.height = _originalHeight; 
            Debug.Log(message:"Player got up");
        }
        else
        {
            _crouched = true;
            _charController.height = +_crouchHeight;
            Debug.Log(message:"Player crouched down");    
        }
    }
}
