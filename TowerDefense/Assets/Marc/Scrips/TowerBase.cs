using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public string Tag;
    public float offset;

    private Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;

    private bool InRange = false;

    void Start()
    {

    }

    void LateUpdate()
    {
        //check if enemy is in Range
        if (InRange)
        {
            //find an enemy with the tag "Enenmy"
            target = GameObject.FindGameObjectWithTag(Tag).GetComponent<Transform>();

            //get the positions of Tower and Enemy
            targetPos = target.position;
            thisPos = transform.position;

            //get distans betwene tower and Enemy
            targetPos.x = targetPos.x - thisPos.x;
            targetPos.y = targetPos.y - thisPos.y;

            //calkulate the radius and the angel of the rotation
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

            //rotate the tower
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        }
        //Draw a line in wich direktion the Tower is looking
        Debug.DrawLine(thisPos, targetPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemy is In Range
        InRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Enemy is out of range 
        InRange = false;
    }
}
