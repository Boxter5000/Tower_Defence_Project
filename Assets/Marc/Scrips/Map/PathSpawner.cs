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
                GameObject Path = Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);

                GameObject PathBranch = transform.parent.transform.Find("Path_T").gameObject;
                PathBranch.SetActive(true);

                GameObject PathReciever = Path.transform.Find("Path_B").gameObject;
                PathReciever.SetActive(true);
            }
            else if (OpeningDirection == 1)
            {
                //Needs opening to the top
                rand = Random.Range(0, templates.topOpening.Length);
                GameObject Path = Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);

                GameObject PathBranch = transform.parent.transform.Find("Path_B").gameObject;
                PathBranch.SetActive(true);

                GameObject PathReciever = Path.transform.Find("Path_T").gameObject;
                PathReciever.SetActive(true);
            }
            else if (OpeningDirection == 2)
            {
                //Needs opening to the left
                rand = Random.Range(0, templates.leftOpening.Length);
                GameObject Path = Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);

                GameObject PathBranch = transform.parent.transform.Find("Path_R").gameObject;
                PathBranch.SetActive(true);

                GameObject PathReciever = Path.transform.Find("Path_L").gameObject;
                PathReciever.SetActive(true);
            }
            else if (OpeningDirection == 3)
            {
                //Needs opening to the right
                rand = Random.Range(0, templates.rightOpening.Length);
                GameObject Path = Instantiate(templates.topOpening[rand], transform.position, Quaternion.identity, Path_Parent.transform);

                GameObject PathBranch = transform.parent.transform.Find("Path_L").gameObject;
                PathBranch.SetActive(true);

                GameObject PathReciever = Path.transform.Find("Path_R").gameObject;
                PathReciever.SetActive(true);
            }
            Instantiate(templates.PathBase, transform.position, Quaternion.identity, Path_Parent.transform);
            Spawned = true;
        }
    }
}
