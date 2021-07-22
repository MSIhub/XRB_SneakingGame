using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    //Path
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;

    //Tolerance between target and agent
    [SerializeField] private float threshold = 0.5f;
    
    private bool _moving = false;
    private Transform _currentPoint;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_moving)
        {
            if (_currentPoint == point1)
            {
                _currentPoint = point2;
            }
            else
            {
                _currentPoint = point1;
            }
            agent.SetDestination(_currentPoint.position);
            _moving = true;
        }

        
        if (_moving &&  Vector3.Distance(transform.position, _currentPoint.position)< threshold)
        {
            _moving = false;
        }

    }
}
