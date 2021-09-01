using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StackACube
{
    public class StackACubeGameManager : MonoBehaviour
    {
        [SerializeField] private float _timeLimit = 5;
        [SerializeField] private GameObject _board;
        [SerializeField] private CubeCornerDrawAndRayCast _targetArea;
        [SerializeField] private List<GameObject> _starList;

        private float _curTime = 0.0f;

        private void Start()
        {
            DimStarRatings(0.3f);
        }

        private void DimStarRatings(float alpha)
        {
            if (_starList.IsNullOrEmpty())
            {
                Debug.LogWarning("Star object list is empty");
            }
            else
            {
                foreach (var star in _starList)
                {
                    if (!star.TryGetComponent<Image>(out var img)) continue;
                    var temp = img.color;
                    temp.a = alpha; //Alpha value must be between 0 and 1
                    img.color = temp;
                    
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (_targetArea.noOfStars==4) return; //Check if max star reached
            if ((_curTime > _timeLimit)) return;
            _curTime += Time.deltaTime;
            UpdateScoreBoard();
            UpdateStarRating();
        }
        
        private void UpdateScoreBoard()
        {
            if (!_board.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI txtFld)) return;
            txtFld.text = "Time : " + (_timeLimit - _curTime).ToString(format:"F0");
            if (_targetArea.noOfStars == 4)
            {
                txtFld.text = "Great Work Champ! Move to next level";
            }
            else if(_curTime >=_timeLimit )
            {
                if(_targetArea.noOfStars ==0)
                {
                    txtFld.text = "Challenge Failed, try again!";
                }
                else if (_targetArea.noOfStars < 4)
                {
                    txtFld.text = "Good job! Move to the next level!";
                }
            }
          
            
        }
        private void UpdateStarRating()
        {
            //including time factor in the score
            var starValue = Mathf.RoundToInt(_targetArea.noOfStars - (0.2f * (_curTime / _timeLimit)));
            for (var i = 0; i < starValue; i++)
            {
                Image img = _starList[i].GetComponent<Image>();
                Color temp = img.color;
                temp.a = 1f; //Alpha value must be between 0 and 1
                img.color = temp;
            }
            //Resetting the star to original if the score changes
            var remainingStar = 4 - starValue;
            for (int i = 0; i < remainingStar; i++)
            {
                Image img = _starList[3-i].GetComponent<Image>();
                Color temp = img.color;
                temp.a = 0.3f; //Alpha value must be between 0 and 1
                img.color = temp;
            }
        }

        public void NextLevel()
        {
            var curLevel = SceneManager.GetActiveScene().buildIndex;
            switch (curLevel)
            {
                case 0:
                    SceneManager.LoadScene(sceneBuildIndex: 1);
                    break;
                case 1:
                    SceneManager.LoadScene(sceneBuildIndex: 2);
                    break;
                case 2:
                    SceneManager.LoadScene(sceneBuildIndex: 0);
                    break;
            }
        }
    }
}
