using Game;
using UnityEngine;
public enum Handed
{
    Left = 0,
    Right = 1
}

public class Handedness : MonoBehaviour
{  
    
    public Handed handed;
    [SerializeField] private GameEventManager _gameManager;
    [SerializeField] private GameObject[] _leftHandedObjects;
    [SerializeField] private GameObject[] _rightHandedObjects;

    private void Awake()
    {
        handed = _gameManager.handedness;
        
        if (handed == Handed.Left)
        {
            foreach (var leftObj in _leftHandedObjects)
            {
                leftObj.SetActive(true);
            }
            foreach (var rightObj in _rightHandedObjects)
            {
                rightObj.SetActive(false);
            }
        }
        else
        {
            foreach (var leftObj in _leftHandedObjects)
            {
                leftObj.SetActive(false);
            }
            foreach (var rightObj in _rightHandedObjects)
            {
                rightObj.SetActive(true);
            }
        }
    }
}
