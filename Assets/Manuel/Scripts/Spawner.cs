using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private Entity enemyPrefab;
    [SerializeField] private Path path;

    private int wave = 1;
    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave() {
        for (float t = 0; t < 3.0f; t += Time.deltaTime) {
            yield return null;
        }
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy() {
        for (int t = 0; t < wave; t++) {
            for (float u = 0; u < 0.25f; u += Time.deltaTime) {
                Entity newEnemy = Instantiate(enemyPrefab, transform);
                newEnemy.SetPath(path);
                yield return null;
            }
        }

        wave++;
        StartCoroutine(SpawnWave());
    }

}
