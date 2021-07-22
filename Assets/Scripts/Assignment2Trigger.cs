using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment2Trigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    Debug.Log(message:"Congrats, you triggered the volume!");
  }
  
  private void OnTriggerExit(Collider other)
  {
    Debug.Log(message:"You are out of trigger volume");
  }
}
