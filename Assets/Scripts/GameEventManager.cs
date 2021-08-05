using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
    public class GameEventManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _failedPanel;
        [SerializeField] private GameObject _successPanel;
        private void Start()
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>(); //Unoptimized way of handling this. Only once used.
            foreach (EnemyController enemy in enemies)
            {
                enemy.onInvestigate.AddListener(EnemyInvestigating);
                enemy.onPlayerFound.AddListener(PlayerFound);
                enemy.onReturnToPatrol.AddListener(EnemyReturnToPatrol);
            }
            
        }

        private void EnemyReturnToPatrol()
        {
            
        }

        private void PlayerFound(Transform enemyThatFoundPlayer)
        {
            
        }

        private void EnemyInvestigating()
        {
            
        }
    }
}