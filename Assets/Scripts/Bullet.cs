using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy)) return;
        enemy.SetStunned();
        Destroy(gameObject);
    }
}
