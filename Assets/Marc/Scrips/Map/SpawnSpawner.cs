using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpawner : MonoBehaviour
{
    /// <summary>
    /// index 0 = top
    /// index 1 = bottom
    /// index 2 = right
    /// index 3 = left
    /// </summary>
    [SerializeField] private Vector2[] spawnPositions = new Vector2[4];

    [SerializeField] private PathSpawner spawnPrefab;
    [SerializeField] private GameObject estatePrefab;

    private Vector2 coordinates = Vector2.zero;

    [SerializeField] private GameObject[] toDestroyOnLayerChange;
    [SerializeField] private GameObject topPath;
    [SerializeField] private GameObject bottomPath;
    [SerializeField] private GameObject rightPath;
    [SerializeField] private GameObject leftPath;

    private static Dictionary<Vector2, SpawnSpawner> fieldPositions;

    private void SpawnFieldButton(int direction) {
        PathSpawner NewSpawner = Instantiate(spawnPrefab, (Vector2)transform.position + spawnPositions[direction], Quaternion.identity, transform);
        switch (direction) {
            case 0:
                NewSpawner.coordinates = coordinates + Vector2.up;
                break;
            case 1:
                NewSpawner.coordinates = coordinates + Vector2.down;
                break;
            case 2:
                NewSpawner.coordinates = coordinates + Vector2.right;
                break;
            case 3:
                NewSpawner.coordinates = coordinates + Vector2.left;
                break;
        }

    }

    public void DestroyForLayerChange() {
        foreach (GameObject go in toDestroyOnLayerChange) {
            Destroy(go);
        }
    }

    public void SpawnFollowingSpawners(Vector2 newCoordinates) {
        coordinates = newCoordinates;
        fieldPositions.Add(coordinates, this);
        for (int i = 0; i < spawnPositions.Length; i++) {
            Vector2 checkPosition = spawnPositions[i] + (Vector2)transform.position;
            Collider2D coll = Physics2D.OverlapBox(checkPosition, Vector2.one, 0.0f);
            if (coll == null) {
                if (checkPosition.x != 0.5d && checkPosition.y != 0.5d) {
                    Instantiate(estatePrefab, (Vector2)transform.position + spawnPositions[i], Quaternion.identity, transform);
                }
                else {
                    SpawnFieldButton(i);
                }
            }
            else {
                if (coll.gameObject.CompareTag("FreeRealEstate")) {
                    Destroy(coll.gameObject);
                    SpawnFieldButton(i);
                }
            }
        }
        SpawnPathsToOthers();
    }

    public void CreateDictionaries() {
        fieldPositions = new Dictionary<Vector2, SpawnSpawner>();
    }

    private void SpawnPathsToOthers() {
        SpawnSpawner above;
        SpawnSpawner below;
        SpawnSpawner toTheRight;
        SpawnSpawner toTheLeft;

        fieldPositions.TryGetValue(coordinates + Vector2.up, out above);
        fieldPositions.TryGetValue(coordinates + Vector2.down, out below);
        fieldPositions.TryGetValue(coordinates + Vector2.right, out toTheRight);
        fieldPositions.TryGetValue(coordinates + Vector2.left, out toTheLeft);

        if(coordinates.y > 0.0f) {
            below.SpawnPath(0);
        }else if(coordinates.y < 0.0f) {
            above.SpawnPath(1);
        }
        if(coordinates.x > 0.0f) {
            toTheLeft.SpawnPath(2);
        }else if (coordinates.x < 0.0f) {
            toTheRight.SpawnPath(3);
        }
        if(Mathf.Abs(coordinates.x) != Mathf.Abs(coordinates.y)) {
            SpawnSpawner ToDestroySpawner = null;
            if (Mathf.Abs(coordinates.x) > Mathf.Abs(coordinates.y)) {
                if (coordinates.x > 0.0f) {
                    fieldPositions.TryGetValue(coordinates + Vector2.left, out ToDestroySpawner);
                }
                else if (coordinates.x < 0.0f) {
                    fieldPositions.TryGetValue(coordinates + Vector2.right, out ToDestroySpawner);
                }
            }else {
                if (coordinates.y > 0.0f) {
                    fieldPositions.TryGetValue(coordinates + Vector2.down, out ToDestroySpawner);
                }
                else if (coordinates.y < 0.0f) {
                    fieldPositions.TryGetValue(coordinates + Vector2.up, out ToDestroySpawner);
                }
            }
            ToDestroySpawner?.DestroyForLayerChange();
        }
    }
    /// <summary>
    /// 0 = topPath
    /// 1 = bottomPath
    /// 2 = rightPath
    /// 3 = leftPath
    /// </summary>
    /// <param name="direction"></param>
    public void SpawnPath(int direction) {
        switch (direction) {
            case 0:
                topPath.SetActive(true);
                break;
            case 1:
                bottomPath.SetActive(true);
                break;
            case 2:
                rightPath.SetActive(true);
                break;
            case 3:
                leftPath.SetActive(true);
                break;
            default:
                break;
        }
    }
}
