using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    //Path
    [SerializeField] private Transform path;
    //Tolerance between target and agent
    [SerializeField] private float threshold = 0.5f;
    
    private bool _moving = false;
    private Transform _currentPoint;

    private List<Transform> _robotPath = new List<Transform>(); // List to store all the child object of Path
    private int itr = 0; // Iterator for the robot path points
    private int _pathSize = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _pathSize = path.childCount; //Number of points in the Path object
        //Extracting each child objects
        foreach (Transform child in path) 
        {
            //if the child object does not have the PointinPath component then its not a path
            //Double layer of protection
            if (child.GetComponent<PointInPath>())
            {
                _robotPath.Add(child);    
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_moving)
        {
            if (itr < _pathSize)
            {
                _currentPoint = _robotPath[itr];
                itr++;
            }
            else
            {
                itr = 0;
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
