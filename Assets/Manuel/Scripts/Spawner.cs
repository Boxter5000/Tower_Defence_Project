using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private Entity enemyPrefab;
    [SerializeField] private float TimeBetweneEnemySpawn = 0.25f;
    public bool AutoStart = false;

    public static int EnemysInScene;

    public int EnemysPerWave = 1;

    void Start()
    {
        FindObjectOfType<MasterSpawner>().AddMeSenpai(this);
    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave() {
        TimeBetweneEnemySpawn = TimeBetweneEnemySpawn / 10f;
        StartCoroutine(SpawnEnemy());
        for (float t = 0; t < 3.0f; t += Time.deltaTime) {
            yield return null;
        }
    }

    private IEnumerator SpawnEnemy() {
        for (int t = 0; t < EnemysPerWave; t++) {
                Entity newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
                //newEnemy.SetPath(path);
                EnemysInScene++;
            for (float u = 0; u < TimeBetweneEnemySpawn; u += Time.deltaTime) {
                yield return null;
            }
        }
        EnemysPerWave += 10000;
    }

    public void SetAutoStart()
    {
        AutoStart = !AutoStart;
    }

    public void EnemyFuckingDied()
    {
        EnemysInScene--;
        if(EnemysInScene <= 0 && AutoStart)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private void OnDisable() {
        FindObjectOfType<MasterSpawner>().RemoveMeSenpai(null);
    }
}
