using System;
using TMPro;
using UnityEngine;

namespace StackACube
{
    public class StackACubeGameManager : MonoBehaviour
    {
        [SerializeField] private float _timeLimit = 5;
        [SerializeField] private GameObject _board;
        [SerializeField] private Transform _targetArea;

        private float _curTime = 0.0f;

        // Update is called once per frame
        void Update()
        {
            _curTime += Time.deltaTime;
            UpdateScoreBoard();
        }
    
        private void UpdateScoreBoard()
        {
            if (!_board.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI txtFld)) return;
            txtFld.text = "Time : " + _curTime.ToString(format:"F1");


            if (_curTime >=_timeLimit)
            {
                txtFld.text = "Challenge Failed, try again!";
            }
        }
        
        //target area
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_targetArea.position, _targetArea.localScale);
        }
    }
}
