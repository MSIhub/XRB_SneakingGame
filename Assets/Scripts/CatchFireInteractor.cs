using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFireInteractor : MonoBehaviour
{
    private ParticleSystem _fireCatcher;

    private void OnTriggerEnter(Collider other)
    {
        _fireCatcher = other.GetComponentInChildren<ParticleSystem>();
        if (_fireCatcher ==null) return;
        var fireEffectEmission = _fireCatcher.emission;
        fireEffectEmission.rateOverTime = 10;
    }
}
