using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    enum EnemyState
    {
        Patrol = 0,
        Investigate = 1,
        Ragdoll = 2
    }
    
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _threshold = 0.5f;   //Tolerance between target and agent
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private PatrolRoute _patrolRoute;
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyState _state = EnemyState.Patrol;

    [SerializeField] private Collider _mainEnemyCollider;
    [SerializeField] private Collider[] _ragdollColliders;
    
    private bool _moving = false;
    private Transform _currentPoint;
    private int _routeIndex = 0;
    private int _routeCount = 0;
    private int _incrementor = 1;
    private Vector3 _investigationPoint;
    private float _waitTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        SetRagdollParts();
        _currentPoint = _patrolRoute.route[_routeIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (_fov.visibleObjects.Count > 0)
        {
            InvestigatePoint(_fov.visibleObjects[0].position);
        }
        if (_state == EnemyState.Patrol)
        {
            UpdatePatrol();    
        }
        else if (_state == EnemyState.Investigate)
        {
            UpdateInvestigate();
        }
        else if (_state == EnemyState.Ragdoll)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        
    }
    
    public void DoRagdoll(bool isRagdoll)
    {
        foreach (var col in _ragdollColliders)
        {
            if (col.gameObject != this.gameObject)
            {
                if (isRagdoll)
                {
                    Debug.Log("Enemy idle");
                    _state = EnemyState.Ragdoll;
                    col.enabled = isRagdoll;
                    Rigidbody rigidbody =col.attachedRigidbody; 
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    rigidbody.isKinematic = !isRagdoll;
                }
                else
                {
                    Rigidbody rigidbody =col.attachedRigidbody; 
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    rigidbody.isKinematic = !isRagdoll;
                    col.enabled = isRagdoll;
                }
            }
        }

        _mainEnemyCollider.enabled = !isRagdoll;

        if (TryGetComponent<Rigidbody>(out Rigidbody _mainEnemyRigidBody))
        {
            _mainEnemyRigidBody.useGravity = !isRagdoll;
        }

        if (TryGetComponent<Animation>(out Animation animation))
        {
            animation.enabled = !isRagdoll;
        }
    }

    public void InvestigatePoint(Vector3 investigatePoint)
    {
        if (_state != EnemyState.Ragdoll)
        {
            //Debug.Log("Investatige Point triggered");
            _state = EnemyState.Investigate;
            _investigationPoint = investigatePoint;
            _agent.SetDestination(_investigationPoint);    
        }
        
    }
    private void SetRagdollParts()
    {
        _mainEnemyCollider = GetComponent<Collider>();
        _ragdollColliders = GetComponentsInChildren<Collider>(true);
        DoRagdoll(false);
    }

    private void UpdateInvestigate()
    {
        //Debug.Log("Investigating");
        if (Vector3.Distance(transform.position, _investigationPoint) < _threshold)
        {
            if (_state != EnemyState.Ragdoll)
            {
                Debug.Log("In Update Investagatin Not IDLE");
                _waitTimer += Time.deltaTime;
                if (_waitTimer > _waitTime)
                {
                    ReturnToPatrol();
                }    
            }
            
        }
        
    }

    private void ReturnToPatrol()
    {
        Debug.Log("Returning to patrol");
        _state = EnemyState.Patrol;
        _waitTimer = 0;
        _moving = false;
    }

    private void UpdatePatrol()
    {
        if (!_moving)
        {
            NextPatrolPoint();
            _agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        if (_moving && Vector3.Distance(transform.position, _currentPoint.position) < _threshold)
        {
            _moving = false;
        }
    }

    private void NextPatrolPoint()
    {
        _currentPoint = _patrolRoute.route[_routeIndex];
        _routeCount = _patrolRoute.route.Count;

        if (_patrolRoute.patrolType == PatrolRoute.PatrolType.Loop)
        {
            _routeIndex = (_routeIndex +1) % _routeCount;
        }
        
        if (_patrolRoute.patrolType == PatrolRoute.PatrolType.PingPong)
        {
            _routeIndex += _incrementor;    //_routeIndex = _routeIndex + incrementor;
            if(_routeIndex == 0 || _routeIndex == _routeCount - 1)
            {
                _incrementor *= -1;    //incrementor = incrementor * -1;
            }
        }
    }


}















/*
//Reverse the direction when it reaches the last point 
if (!_forwardAlongPath | _routeIndex == _routeCount - 1)
{
    _forwardAlongPath = false;
    if (_firstIteration)
    {
        _firstIteration = false; // to ensure that the last point is visited before returning
        return;
    }
    _routeIndex = (((_routeIndex - 1) % _routeCount) +_routeCount) % _routeCount ;
    if (_routeIndex == 0)
    {
        _forwardAlongPath = true;
        _firstIteration = true;
    }
}
*/

/*
if (_forwardAlongPath)
{
    _routeIndex++;    
}
else
{
    _routeIndex--;
}

if (_routeIndex == 0)
{
    _forwardAlongPath = true;
}

if (_routeIndex == _patrolRoute.route.Count)
{
    if (_patrolRoute.patrolType == PatrolRoute.PatrolType.Loop)
    {
        _routeIndex = 0;    
    }
    else if (_patrolRoute.patrolType == PatrolRoute.PatrolType.PingPong)
    {
        _forwardAlongPath = false;
        _routeIndex--;
    }
}*/