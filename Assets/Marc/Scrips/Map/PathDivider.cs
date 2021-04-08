using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDivider : PathStart {
    [SerializeField] private Path leftPath;
    [SerializeField] private Path rightPath;

    private bool goLeft = false;
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject EnemyCollidet = collision.gameObject;

        if (EnemyCollidet != null) {
            Entity entity = EnemyCollidet.GetComponent<Entity>();
            if (goLeft) {
                entity?.SetPath(leftPath);
            }
            else {
                entity?.SetPath(rightPath);
            }
            goLeft = !goLeft;
        }
    }
}
