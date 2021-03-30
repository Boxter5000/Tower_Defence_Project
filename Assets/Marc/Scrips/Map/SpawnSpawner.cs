using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpawner : MonoBehaviour {
    /// <summary>
    /// index 0 = top
    /// index 1 = bottom
    /// index 2 = left
    /// index 3 = right
    /// </summary>
    [SerializeField] private Vector2[] spawnPositions = new Vector2[4];

    [SerializeField] private GameObject spawnPrefab;

    private void Awake() {
        for(int i = 0; i < spawnPositions.Length; i++) {
            Collider2D coll = Physics2D.OverlapBox(spawnPositions[i], Vector2.one, 0.0f);
            if(coll != null) {
                Instantiate(spawnPrefab, (Vector2)transform.position + spawnPositions[i], Quaternion.identity, transform);
            }
        }
    }
}
