using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PatrolRoute : MonoBehaviour
    {
        public enum PatrolType
        {
            Loop = 0,
            PingPong = 1
        }

        [SerializeField] private Color _patrolRouteColor;
        public PatrolType patrolType;
        public List<Transform> route;
        
        //Add point from the inspector
        [Button(name:"Add Patrol Point")] //Add button with Odin Inspector can be done also without Odin
        private void AddPatrolPoint()
        {
            GameObject thisPoint = new GameObject(); //Easy way to instantiate an empty gameobject whereas GameObject.Instantiate() takes a prefab as input
            var transform1 = transform;
            thisPoint.transform.position = transform1.position; //Setting the position as the Objects postion
            thisPoint.transform.parent = transform1; //Parents the new object into the parent of the object
            thisPoint.name = "Point" + (route.Count + 1);//Renames the gameobject

            #if UNITY_EDITOR 
                Undo.RegisterCreatedObjectUndo(thisPoint, name:"Add Patrol Point"); //Add the function to Undo list also so when ctrl Z is pressed
            #endif
            
            route.Add(thisPoint.transform); //add the route in list
            
        }
        
        
        //Reversing the patrol route
        [Button(name: "Reverse Patrol Route")]
        private void ReversePatrolRoute()
        {
            route.Reverse();
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR //Build only on Unity Editor 
                        Handles.Label(transform.position, gameObject.name); //Handle is a unity editor script
            #endif
        }

        //draws in the gizmos in the scene mode 
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _patrolRouteColor;

            for (int i = 0; i < route.Count-1; i++)
            {
                Gizmos.DrawLine(route[i].position, route[i+1].position);
                
            }

            if (patrolType == PatrolType.Loop)
            {
                Gizmos.DrawLine(route[route.Count-1].position, route[0].position);
            }
        }
    }
}