using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpawner : MonoBehaviour
{
    List<Spawner> SpawnerList = new List<Spawner>();

    UIController DripController;

    int CurrentWave = 1;

    private void Start() {
        DripController = FindObjectOfType<UIController>();
    }

    public void AddMeSenpai(Spawner NewSpawner) {
        SpawnerList.Add(NewSpawner);
    }

    public void RemoveMeSenpai(Spawner CurrentSpawner) {
        if (SpawnerList.Contains(CurrentSpawner)) {
            SpawnerList.Remove(CurrentSpawner);
        }
    }

    public void StartTheArmageddon() {
        if(Spawner.EnemysInScene<= 0) {
            StartWaves();
        }
    }

    private void StartWaves() {
        DripController.UpdateWave(CurrentWave);
        foreach (Spawner sp in SpawnerList) {
            sp.StartNextWave();
        }
        CurrentWave++;
    }
}
