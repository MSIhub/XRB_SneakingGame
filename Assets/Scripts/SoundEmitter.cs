using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class SoundEmitter : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private float _soundRadius = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _soundRadius);
        foreach (var col in _colliders)
        {
            if (col.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.InvestigatePoint(transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _soundRadius);
    }
}
