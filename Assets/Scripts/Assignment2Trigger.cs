using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment2Trigger : MonoBehaviour
{
  [SerializeField] private GameObject canv;
  //[SerializeField] private GameObject canvasHide;

  private CanvasActionOnTrigger[] _canvToDisp;
  private bool canvExist = false;
  
  
  // Start is called before the first frame update
  void Start()
  {
      _canvToDisp = canv.GetComponentsInChildren<CanvasActionOnTrigger>();
      canvExist = _canvToDisp.Length > 0 ? true : false;
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
      if (canvExist)
      {
          foreach (var child1 in _canvToDisp)
          {
              if (child1.display)
              {
                  child1.gameObject.SetActive(true);    
              }
              else if (child1.hide)
              {
                  child1.gameObject.SetActive(false);
              }
          
          }    
      }
  }

  private void HideCanvas()
  {
      if (canvExist)
      {
          foreach (var child1 in _canvToDisp)
          {
              if (child1.display)
              {
                  child1.gameObject.SetActive(false);    
              }
              else if (child1.hide)
              {
                  child1.gameObject.SetActive(true);
              }
          
          }    
      }
  }
}

