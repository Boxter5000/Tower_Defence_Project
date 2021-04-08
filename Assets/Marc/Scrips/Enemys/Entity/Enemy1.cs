using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float HitPoints;
    Spawner spawner;
    bool isAlive = true;

    float DamageReceved;

    private void Awake()
    {
        spawner = transform.parent.GetComponent<Spawner>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageReceved = collision.gameObject.GetComponent<Bullet1>().Damage;

        HitPoints -= DamageReceved;
        if(HitPoints <= 0 && isAlive)
        {
            isAlive = false;
        spawner.EnemyFuckingDied();
        Destroy(gameObject);
        }
    }
}
