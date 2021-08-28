using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BladeTunner : MonoBehaviour
{
    [SerializeField] private Transform _bladePivot;
    [SerializeField] private float _speedStep = 0.005f;
    [SerializeField] private float _startAngle = 25f;
    [SerializeField] private float _stopAngle = -25f;

    private float _slerpValue;
    private Quaternion _startRot;
    private Quaternion _stopRot;
    

    // Start is called before the first frame update
    void Start()
    {
        _slerpValue = 0.0f;
        _startRot = Quaternion.Euler(0f, 0f, _startAngle);
        _stopRot = Quaternion.Euler(0f, 0f, _stopAngle);

    }

    // Update is called once per frame
    void Update()
    {
        if (_slerpValue >1f)
        {
            _slerpValue = 0.0f;
            (_startRot, _stopRot) = (_stopRot, _startRot);
        }
        _slerpValue += _speedStep;
        _bladePivot.rotation = Quaternion.Slerp(_startRot,_stopRot,_slerpValue );
    }
}
