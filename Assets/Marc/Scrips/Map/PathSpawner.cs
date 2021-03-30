using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{
    public int OpeningDirection;
    // 1 --> Path on bottom
    // 0 --> Path on top
    // 2 --> Path on left
    // 3 --> Path on right

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
            if(OpeningDirection == 0)
            {
                //Needs opening to the bottom
                rand = Random.Range(0, templates.bottomOpening.Length);
                Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
            }
            else if (OpeningDirection == 1)
            {
                //Needs opening to the top
                rand = Random.Range(0, templates.topOpening.Length);
                Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
            }
            else if (OpeningDirection == 2)
            {
                //Needs opening to the left
                rand = Random.Range(0, templates.leftOpening.Length);
                Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
            }
            else if (OpeningDirection == 3)
            {
                //Needs opening to the right
                rand = Random.Range(0, templates.rightOpening.Length);
                Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);
            }
            Spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("SpawnPoint"))
        {
            if(!collision.GetComponent<PathSpawner>().Spawned && !Spawned)
            {
                Instantiate(templates.ClosedRome, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            Spawned = true;
        }
    }
}
