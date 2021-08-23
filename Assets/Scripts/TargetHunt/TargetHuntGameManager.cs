using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TargetHunt
{
    public class TargetHuntGameManager : MonoBehaviour
    {
        [SerializeField] private List<MovingTarget> _movingTargets;
        [SerializeField] private GameObject _scoreBoard;
       // [SerializeField] private ResetMeshInteractor _resetMesh;
        private Transform[] _targetMeshes;
        private int _score;
        private bool[] _hitTargets;

        private int _index = 0;

        // Start is called before the first frame update
        void Start()
        {
            _targetMeshes = new Transform[_movingTargets.Count];
            _hitTargets = new bool[_movingTargets.Count];
            foreach (var target in _movingTargets)
            {
                foreach (Transform child in target.transform)
                {
                    if (child.CompareTag("MovingTarget"))
                    {
                        _targetMeshes[_index] = child;
                    }

                }

                _index++;
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateScore();
            UpdateScoreBoard();
        }

        private void UpdateScore()
        {
            for (int i = 0; i < _targetMeshes.Length; i++)
            {
                if (_targetMeshes[i].GetComponent<TargetInteract>().isHit)
                {
                    _hitTargets[i] = true;
                }
            }

            int ctScore = 0;
            foreach (var ht in _hitTargets)
            {
                if (ht)
                {
                    ctScore += 1;
                }
            }

            _score = ctScore;
        }

        private void UpdateScoreBoard()
        {
            if (!_scoreBoard.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI txtFld)) return;
            if (_score > 0)
            {
                txtFld.text = "Score : " + _score.ToString();
            }

            if (_score == _targetMeshes.Length)
            {
                txtFld.text = "Congratulations Champ 3/3!";
            }
        }
    }
    
}
