using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpawner : MonoBehaviour
{
    /// <summary>
    /// index 0 = top
    /// index 1 = bottom
    /// index 2 = left
    /// index 3 = right
    /// </summary>
    [SerializeField] private Vector2[] spawnPositions = new Vector2[4];

    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private GameObject estatePrefab;

    private int OpeningDir;

    private void Awake()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Vector2 checkPosition = spawnPositions[i] + (Vector2)transform.position;
            Collider2D coll = Physics2D.OverlapBox(checkPosition, Vector2.one, 0.0f);
            if (coll == null)
            {
                if (checkPosition.x != 0.5d && checkPosition.y != 0.5d)
                {
                    Instantiate(estatePrefab, (Vector2)transform.position + spawnPositions[i], Quaternion.identity, transform);
                }
                else
                {
                    GameObject NewSpawner = Instantiate(spawnPrefab, (Vector2)transform.position + spawnPositions[i], Quaternion.identity, transform);
                    NewSpawner.GetComponent<PathSpawner>().OpeningDirection = i;
                }
            }
            else
            {
                if (coll.gameObject.CompareTag("FreeRealEstate"))
                {
                    Destroy(coll.gameObject);
                    GameObject NewSpawner = Instantiate(spawnPrefab, (Vector2)transform.position + spawnPositions[i], Quaternion.identity, transform);
                    NewSpawner.GetComponent<PathSpawner>().OpeningDirection = i;
                }
            }
        }
    }
}
