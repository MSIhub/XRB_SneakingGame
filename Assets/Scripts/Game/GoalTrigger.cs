using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class GoalTrigger : MonoBehaviour
    {
        public UnityEvent onGoalReached;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<Rigidbody>().gameObject.CompareTag("Player")) return;
            onGoalReached.Invoke();
        }
    }
}