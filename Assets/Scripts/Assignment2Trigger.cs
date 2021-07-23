using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment2Trigger : MonoBehaviour
{
  [SerializeField] private GameObject textField;
  
  // Start is called before the first frame update
  void Start()
  {
      textField.SetActive(false);
  }
  
  private void OnTriggerEnter(Collider other)
  {
     Debug.Log(message:"Congrats, you triggered the volume!");
     textField.SetActive(true);
  }
  private void OnTriggerExit(Collider other)
  {
      Debug.Log(message:"You are out of trigger volume");
      textField.SetActive(false);
  }

}
