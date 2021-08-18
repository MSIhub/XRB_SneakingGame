using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorKeyCardTrigger : DoorTrigger
{
    [SerializeField] private int _keyCardLevel = 1;
    [SerializeField] private XRSocketInteractor _socket;
    [SerializeField] private Renderer _lightObject;
    [SerializeField] private Light _light;
    [SerializeField] private Color _defaultColor = Color.yellow;
    [SerializeField] private Color _errorColor = Color.red;
    [SerializeField] private Color _successColor = Color.green;
    
    
    private bool _isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        SetLightColor(_defaultColor);
        _socket.selectEntered.AddListener(KeyCardInserted);
        _socket.selectExited.AddListener(KeyCardRemoved);
    }
    private void KeyCardInserted(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactable.TryGetComponent(out KeyCard keyCard))
        {
            Debug.LogWarning("No Keycard componernt attached to inserted object");
            SetLightColor(_errorColor);
            return;
        }

        if (keyCard.keyCardLevel >= _keyCardLevel)
        {
            SetLightColor(_successColor);
            _isOpen = true;
            OpenDoor();
        }
        else
        {
            SetLightColor(_errorColor);
        }
    }
    private void KeyCardRemoved(SelectExitEventArgs arg0)
    {
        SetLightColor(_defaultColor);
        _isOpen = false;
        CloseDoor();
    }

   

    protected override void OnTriggerExit(Collider other)
    {
        if (_isOpen) return;
        base.OnTriggerExit(other);    
    }

    private void SetLightColor(Color color)
    {
        _lightObject.material.color = color;
        _light.color = color;
    }
}
