using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathStart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject EnemyCollidet = collision.gameObject;

        if(EnemyCollidet != null)
        {
            Entity entity = EnemyCollidet.GetComponent<Entity>();
            entity?.SetPath(transform.parent.GetComponent<Path>());
        }
    }
}
