using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using StarterAssets;
using UnityEngine;

public class Assignment2Trigger : MonoBehaviour
{
  [SerializeField] private GameObject obj;
  [SerializeField]private float _waitTimeInSeconds = 10.0f;
  
  private float _timer = 0.0f;
  private bool _timerStarted = false;
  private Vector3 _objInitialPosition;
  private Quaternion _objInitialRotation;
  private Vector3 _objInitialScale;
  private void Start()
  {
    obj.SetActive(false);
    _objInitialPosition = obj.transform.localPosition;
    _objInitialRotation = obj.transform.localRotation;
    _objInitialScale = obj.transform.localScale;

  }

  private void Update()
  {
    if (_timerStarted)
    {
      _timer += Time.deltaTime;
    }
    
    if (_timer > _waitTimeInSeconds)
    {
      obj.SetActive(true);
      _timerStarted = false;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<FirstPersonController>())
    {
      if (!_timerStarted)
      {
        _timerStarted = true;
      } 
    }
  }
  
  private void OnTriggerExit(Collider other)
  {
    if (other.GetComponent<FirstPersonController>())
    {
      obj.transform.localPosition = _objInitialPosition;
      obj.transform.localRotation = _objInitialRotation;
      obj.transform.localScale = _objInitialScale;
      obj.SetActive(false);
      _timerStarted = false;
      _timer = 0.0f;
    }
  }
}
