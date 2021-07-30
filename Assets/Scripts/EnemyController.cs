using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PatrolRoute _patrolRoute;
    [SerializeField] private float _threshold = 0.5f;    //Tolerance between target and agent
    
    private bool _moving = false;
    private Transform _currentPoint;
    private int _routeIndex = 0;
    private bool _forwardAlongPath = true;
    private int _routeCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentPoint = _patrolRoute.route[_routeIndex];
        
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!_moving)
        {
            NextPatrolPoint();
            _agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        
        if (_moving &&  Vector3.Distance(transform.position, _currentPoint.position)< _threshold)
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
            if (_forwardAlongPath)
            {
                _routeIndex = (_routeIndex +1) % _routeCount;   
            }
            //Reverse the direction when it reaches the last point 
            if (!_forwardAlongPath | _routeIndex == _routeCount - 1)
            {
                if (_routeIndex == 0)
                {
                    _forwardAlongPath = true;                    
                }
                _forwardAlongPath = false;
                _routeIndex = (((_routeIndex - 1) % _routeCount) +_routeCount) % _routeCount ;
            }
        }

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
    }
}
