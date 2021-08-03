using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StickOnThrow : MonoBehaviour
{
    public bool triggerSticking = false;
    [SerializeField] private GameObject _particleEffect;
    [SerializeField] private float _bombRange = 5.0f;
    private bool _stuckToWall = false;
    private Vector3 pointOnObject = Vector3.zero;
    private ContactPoint[] _contactPoints = new ContactPoint[6];//6 vertices of possible contacts for my mine
    private void Start()
    {
        _particleEffect.SetActive(false);
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
                ExplodeCreature(targetCreature);
            }
        }
    }

    private void ExplodeCreature(Creature targetCreature)
    {
        Transform objTransform = transform;
        _particleEffect.transform.position = objTransform.position;
        _particleEffect.transform.rotation = objTransform.rotation;
        _particleEffect.SetActive(true);
        targetCreature.transform.gameObject.SetActive(false);

    }

    private void OnCollisionEnter(Collision other)
    {
        StickToCollidedSurfaceOnThrow(other);
        _stuckToWall = true;
    }

    private void StickToCollidedSurfaceOnThrow(Collision other)
    {
        if (!triggerSticking) return;
        // Extracting the mean of contact points 
        var meanContactPoint = MeanContactPoint(other);
        FixedJointCreateAtAnchor(other, meanContactPoint);
    }

    private void FixedJointCreateAtAnchor(Collision other, Vector3 meanContactPoint)
    {
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.anchor = meanContactPoint;
        joint.connectedBody = other.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
        joint.enableCollision = false;
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
