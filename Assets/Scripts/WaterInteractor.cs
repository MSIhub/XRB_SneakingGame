using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteractor : MonoBehaviour
{
    private ParticleSystem _fireEffect;
    private void OnTriggerEnter(Collider other)
    {
        _fireEffect = other.GetComponentInChildren<ParticleSystem>();
        if (_fireEffect ==null) return;
        var fireEffectEmission = _fireEffect.emission;
        fireEffectEmission.rateOverTime = 0;
        
    }
}
