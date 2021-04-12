using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{
    public Vector2 coordinates = Vector2.zero;
    // higher Value = layer
    // other Value = distance from default axis


    PathTemplates templates;
    private int rand;
    private bool Spawned = false;
    private GameObject Path_Parent;


    private void Awake()
    {
        //Invoke("Spawn", 2f);
        templates = GameObject.FindGameObjectWithTag("PathTemplate").GetComponent<PathTemplates>();
        Path_Parent = GameObject.FindGameObjectWithTag("GridParant");
    }

    private void OnMouseDown()
    {
        Spawn();
    }

    void Spawn()
    {
        if (!Spawned)
        {
            int sector = 0; 
            if (coordinates.x > 0.0f) {
                sector += 3;
            } else if (coordinates.x < 0.0f) {
                sector -= 3;
            }
            if (coordinates.y > 0.0f) {
                sector += 2;
            } else if (coordinates.y < 0.0f) {

                sector -= 2;
            }
            Instantiate(templates.PathBase, transform.position, Quaternion.identity, Path_Parent.transform);
            SpawnSpawner Path = null;
            rand = Random.Range(0, templates.bottomLeftOpening.Length);
            switch (sector) {
                case -5:
                    Path = Instantiate(templates.topRightOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                case -3:
                    Path = Instantiate(templates.rightOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                case -2:
                    Path = Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                case -1:
                    Path = Instantiate(templates.bottomRightOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                case 0:
                    Path = null;
                    break;
                case 1:
                    Path = Instantiate(templates.topLeftOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                case 2:
                    Path = Instantiate(templates.bottomOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                case 3:
                    Path = Instantiate(templates.leftOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                case 5:
                    Path = Instantiate(templates.bottomLeftOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
                    break;
                default:
                    break;
            }
            Path?.SpawnFollowingSpawners(coordinates);
            Spawned = true;
        }
    }
}