using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class StickOnThrow : MonoBehaviour
{
    public enum StickOnThrowFunction
    {
        Explode = 0,
        Ragdoll =1
    }
    
    public bool triggerSticking = false;
    [SerializeField] private GameObject _particleEffect;
    [SerializeField] private float _bombRange = 5.0f;
    [SerializeField] private float _angleToSnap = 90.0f;
    [SerializeField] private EnemyController _robotToRagdoll;
    [SerializeField] private StickOnThrowFunction _throwFunction = StickOnThrowFunction.Explode;

    private AudioSource _audioSource;
    private Rigidbody _mineObject;
    private Vector3 _meanContactPoint;
    private bool _stuckToWall = false;
    private Vector3 pointOnObject = Vector3.zero;
    private ContactPoint[] _contactPoints = new ContactPoint[6]; //Maximum of 6 vertices to the face
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _particleEffect.SetActive(false);
        _mineObject = this.GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        if (!_stuckToWall) return;
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.up, out hit, _bombRange)) return;
        if (hit.transform.TryGetComponent<Creature>(out Creature targetCreature))
        { 
            if (targetCreature.team == Creature.Team.Enemy)
            {
                if (_throwFunction == StickOnThrowFunction.Explode)
                {
                    ExplodeCreature(targetCreature);  
                }
                else if (_throwFunction == StickOnThrowFunction.Ragdoll)
                {
                    _robotToRagdoll.DoRagdoll(true);
                }
            }
        }
    }
    

    private void ExplodeCreature(Creature targetCreature)
    {
        _audioSource.Play(); 
        Transform objTransform = transform;
        _particleEffect.transform.position = objTransform.position;
        _particleEffect.transform.rotation = objTransform.rotation;
        _particleEffect.transform.localScale = new Vector3(5f, 5f, 5f);
        _particleEffect.SetActive(true);
        targetCreature.transform.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!triggerSticking) return;// Ensure sticking is enabled
        StickToCollidedSurface(other);
    }

    private void StickToCollidedSurface(Collision other)
    {
        _meanContactPoint = MeanContactPoint(other); // take mean of all the contact points
        _mineObject.transform.position = _meanContactPoint; //StickToCollidedSurfaceOnThrow
        _mineObject.isKinematic = true; // Freeze the motion
        _mineObject.transform.parent = other.transform.parent; //parent the object thrown to the object it collided
        Vector3 forwardVector = _mineObject.transform.forward; //Get the forward direction of the object thrown
        //Based on the direction, the axis of rotation is determined (Assign the maximum value the axis of rotation)
        Vector3 directionVector;
        if (forwardVector.x >= forwardVector.y)
            directionVector = forwardVector.x >= forwardVector.z ? new Vector3(1.0f,0f,0f) : new Vector3(0f,0f,1.0f);
        else
            directionVector = forwardVector.y >= forwardVector.z ? new Vector3(0f,1.0f,0f) : new Vector3(0f,0f,1.0f);
        //Set the rotation 
        _mineObject.transform.rotation = Quaternion.Euler(directionVector*_angleToSnap);
        _stuckToWall = true;
    }
    
    private Vector3 MeanContactPoint(Collision other)
    {
        other.GetContacts(_contactPoints);
        for (int i = 0; i < other.contactCount; i++)
        {
            pointOnObject += _contactPoints[i].point;
        }

        Vector3 meanContactPoint = pointOnObject / other.contactCount;
        return meanContactPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.up);
    }
}
