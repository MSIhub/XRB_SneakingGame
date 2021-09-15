using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class ViewCulling : MonoBehaviour
{
    private FieldOfView _fov;
    private List<Transform> _enemiesInViewLastFrame;
    private void Start()
    {
        _fov = GetComponent<FieldOfView>();
        _enemiesInViewLastFrame = new List<Transform>();

    }

    private void Update()
    {   
        //out of view
        var enemiesOutOfViewNow = _enemiesInViewLastFrame.Except(_fov.visibleObjects).ToList(); //LINQ
        foreach (var enemy in enemiesOutOfViewNow)
        {
            Debug.Log(enemy.name + " is out of view");
            var rend = enemy.GetComponentInChildren<SkinnedMeshRenderer>();
            if (rend)
            {
                rend.enabled = false;
            }
        }
        //Is in view
        var enemiesInViewNow = _fov.visibleObjects.Except(_enemiesInViewLastFrame).ToList();//LINQ
        foreach (var enemy in enemiesInViewNow)
        {
            Debug.Log(enemy.name + " is in view");
            var rend = enemy.GetComponentInChildren<SkinnedMeshRenderer>();
            if (rend)
            {
                rend.enabled = true;
            }
        }
        _enemiesInViewLastFrame = new List<Transform>(_fov.visibleObjects);//copy a list to another instead of just storing the address

    }
}
