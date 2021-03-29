using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int HitPoints;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitPoints--;
        if(HitPoints <= 0)
        {
        Destroy(gameObject);
        }
    }
}
