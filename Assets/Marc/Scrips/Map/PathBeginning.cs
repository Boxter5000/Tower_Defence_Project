using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBeginning : MonoBehaviour
{
    [SerializeField] private SpawnSpawner instance;

    private void Awake() {
        instance.CreateDictionaries();
        instance.SpawnFollowingSpawners(Vector2.zero);
    }
}
