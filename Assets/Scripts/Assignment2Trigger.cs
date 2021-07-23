using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment2Trigger : MonoBehaviour
{
  [SerializeField] private Canvas canvasDisplay;
  [SerializeField] private Canvas canvasHide;
  
  
  // Start is called before the first frame update
  void Start()
  {
      CloseAllCanvas();
  }
  
  private void OnTriggerEnter(Collider other)
  {
     Debug.Log(message:"Congrats, you triggered the volume!");
     ActivateCanvas();
  }
  private void OnTriggerExit(Collider other)
  {
      Debug.Log(message:"You are out of trigger volume");
      CloseAllCanvas();
  }

  private void ActivateCanvas()
  {
      canvasHide.gameObject.SetActive(false);
      canvasDisplay.gameObject.SetActive(true);
  }

  private void CloseAllCanvas()
  {
      canvasHide.gameObject.SetActive(true);
      canvasDisplay.gameObject.SetActive(false);
  }
}
//#TODO: Receive Canvas list and handle multiple canvas to display and hide 