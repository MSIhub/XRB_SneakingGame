using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinCanGameManger : MonoBehaviour
{
    [SerializeField] private CanInteractor[] _canPyramid;

    //[SerializeField] private bool _allCansFeel = false;
    // Start is called before the first frame update
    void Start()
    {
        _canPyramid = gameObject.GetComponentsInChildren<CanInteractor>();
        if (_canPyramid.Length < 1)
        {
            Debug.LogWarning("Can pyramid is missing");
        }
        Debug.Log(_canPyramid.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckAllCansFeel()) return;
        Debug.Log("Congratulations");
    }

    private bool CheckAllCansFeel()
    {
        int check = 0;
        foreach (var can in _canPyramid)
        {
            if (can.canFeel == true)
                check += 1;
        }

        return (check == _canPyramid.Length);
            
    }
}
