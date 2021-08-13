using Sirenix.OdinInspector;
using UnityEngine;

namespace RestrictedAccess
{
    public class AccessPadCheck : MonoBehaviour
    {
        [SerializeField] private string _uniqueIdentifier = "xrbt1";
        [SerializeField] private GameObject _door;
        [SerializeField] private float _resetTime= 2.0f;

        [InfoBox("Below values are specific for the mesh of the access pad and key card")]
        [ShowInInspector, ReadOnly] private float _distanceToCheck = 0.2f;
        [ShowInInspector, ReadOnly] private float _sphereCastRadius = 0.2f;
        [ShowInInspector, ReadOnly] private Vector3 _keyCardSnapOffset = new Vector3(-0.11f, 0f, 0.03f);
    
        private KeyCardHolder _keyCardHolder;
        private bool _isKeyAttached = false;
        private DoorTrigger _doorTrigger;
        private DoorMesh _doorMesh;
        private float _resetTimer = 0.0f;

        private void Start()
        {
        
            _doorTrigger= _door.GetComponentInChildren<DoorTrigger>();
            _doorMesh = _door.GetComponentInChildren<DoorMesh>();
        
            // To enable door access only through the access pad functionality, set the door trigger to false.
            if (_doorTrigger !=null)
            {
                _doorTrigger.gameObject.SetActive(false);    
            }
        }

        private void FixedUpdate()
        {
            if (!_isKeyAttached)
            {
                RaycastHit hit;
                if(Physics.SphereCast(transform.position, _sphereCastRadius,transform.TransformDirection(Vector3.up), out hit, _distanceToCheck))
                {
                    Debug.Log("Did Hit"+ hit.transform.name);
                    if (hit.transform.gameObject.TryGetComponent<KeyCardHolder>(out _keyCardHolder))
                    {
                        if (_uniqueIdentifier == _keyCardHolder.uniqueIdentifierKeyCard)
                        {
                            AttachKey();
                        }
                    }
                }    
            }
        
        
            // Reset the objects after _resetTime upon key attached
            if (_isKeyAttached)
            {
                _resetTimer += Time.deltaTime;
                if (_resetTimer > _resetTime)
                {
                    ResetAccessObjects();
                    _isKeyAttached = false;
                    _resetTimer = 0.0f;
                }    
            }
        
        }

        private void AttachKey()
        {
            Transform keyCard = _keyCardHolder.transform; 
            if (keyCard.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
            keyCard.parent = transform;
            keyCard.localPosition = Vector3.zero + _keyCardSnapOffset; //new Vector3(-0.105f, -0.01f, 0.025f);  //based on the geometry of the key card and the access holder
            keyCard.localRotation = Quaternion.Euler(Vector3.zero);
            OpenDoor(true);
            _isKeyAttached = true;
        }

        private void OpenDoor(bool openDoor)
        { 
            _doorMesh.gameObject.SetActive(!openDoor);    
        }

        private void ResetAccessObjects()
        {
            OpenDoor(false);
            //resetting to initial position
            var transform1 = _keyCardHolder.transform;
            transform1.parent = transform.parent;
            transform1.position = _keyCardHolder.initPositionKeyCard;
            transform1.rotation = _keyCardHolder.initRotationKeyCard;
            //Enabling physics interaction for grabbing
            if ( _keyCardHolder.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }
        }
    }
}
