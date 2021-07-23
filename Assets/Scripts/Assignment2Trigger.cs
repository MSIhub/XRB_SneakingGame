using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment2Trigger : MonoBehaviour
{
  [SerializeField] private GameObject canvasDisplay;
  [SerializeField] private GameObject canvasHide;

  private CanvasAction[] _canvToDisp;
  private CanvasAction[] _canvToHide;
  private bool _dispCanvExist = false;
  private bool _hideCanvExist = false;
  
  // Start is called before the first frame update
  void Start()
  {
      _canvToDisp = canvasDisplay.GetComponentsInChildren<CanvasAction>();
      _canvToHide = canvasHide.GetComponentsInChildren<CanvasAction>();
      _dispCanvExist = _canvToDisp.Length > 0 ? true : false;
      _hideCanvExist = _canvToHide.Length > 0 ? true : false;
      HideCanvas();
  }
  

  private void OnTriggerEnter(Collider other)
  {
      DisplayCanvas();     
  }
  private void OnTriggerExit(Collider other)
  {
      HideCanvas();
  }

  private void DisplayCanvas()
  {
      if (_dispCanvExist)
      {
          foreach (var child1 in _canvToDisp)
          {
              Debug.Log(child1);
              child1.gameObject.SetActive(true);
          }
      }

      
      if (_hideCanvExist)
      {
          foreach (var child2 in _canvToHide)
          {
              Debug.Log(child2);
              child2.gameObject.SetActive(false);
          }
      }
  }

  private void HideCanvas()
  {
      if (_dispCanvExist)
      {
          foreach (var child1 in _canvToDisp)
          {
              Debug.Log(child1);
              child1.gameObject.SetActive(false);
          }
      }

      if (_hideCanvExist)
      {
          foreach (var child2 in _canvToHide)
          {
              Debug.Log(child2);
              child2.gameObject.SetActive(true);
          }
      }
  }
}

