using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDivider : PathStart {
    [SerializeField] private Path leftPath;
    [SerializeField] private Path rightPath;

    private void Update() {
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (Random.Range(0, 2) == 0) {
            entity?.SetPath(leftPath);
        }
        else { 
            entity?.SetPath(rightPath);
        }
    }
}
