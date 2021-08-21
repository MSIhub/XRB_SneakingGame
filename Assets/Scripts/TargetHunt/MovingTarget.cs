using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _stopPoint;
    [SerializeField] private float _speed = 1.5f;
    private Vector3 _startPosition;
    private Vector3 _stopPosition;
    private Vector3 _beginPosition;
    private Vector3 _cX = Vector3.zero;
    private Vector3 _dirToMoveBeginStart;
    private Vector3 _dirToMoveStartStop;
    private Vector3 _dirToMoveStopStart;
    private Vector3 _dirToMove;


    // Start is called before the first frame update
    void Start()
    {
        _beginPosition = transform.position;
        _startPosition = _startPoint.position;
        _stopPosition = _stopPoint.position;

        _dirToMoveBeginStart = (_startPosition - _beginPosition).normalized;
        _dirToMoveStartStop = (_stopPosition - _startPosition).normalized;
        _dirToMoveStopStart = (_startPosition - _stopPosition).normalized;
        _dirToMove = _dirToMoveBeginStart;
    }

    // Update is called once per frame
    void Update()
    {
        _cX = transform.position; //current position
        var dX = _dirToMove * _speed * Time.deltaTime; // delta position = vel * deltatime
        _cX += dX; // current+delta
        transform.position = _cX;
        if (Vector3.Distance(transform.position, _startPosition) < 0.1f)
        {
            _dirToMove = _dirToMoveStartStop;
        }

        if (Vector3.Distance(transform.position, _stopPosition) < 0.1f)
        {
            _dirToMove = _dirToMoveStopStart;
        }
    }
}

// Using Lerp
/*if (_initRun)
{
    _factor += (0.002f*_lerpSpeed);
    transform.position = Vector3.Lerp(_beginPosition, _startPosition, _factor);
    if (Vector3.Distance(transform.position, _startPosition) < 0.1f)
    {
        _initRun = false;
        _factor = 0.0f;
    }
}
if (!_initRun)
{
    if (_factor > 1)
    {
        _factor = 0.0f;
        (_startPosition, _stopPosition) = (_stopPosition, _startPosition);
    }
    _factor += (0.002f*_lerpSpeed);
    transform.position = Vector3.Lerp(_startPosition, _stopPosition, _factor);
}*/