using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public string Tag;
    public float offset;

    public GameObject Bullet;
    public float BulletVelocity;

    private Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;

    public bool InRange = false;

    private void Start()
    {
        //find an enemy with the tag "Enenmy"
        target = GameObject.FindGameObjectWithTag(Tag).GetComponent<Transform>();
    }
    void LateUpdate()
    {
        //check if enemy is in Range
        if (InRange)
        {
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
        Debug.DrawLine(thisPos, target.position);
    }

    IEnumerator shootTimer()
    {
        for (int i = 0; InRange; i += i)
        {
            yield return new WaitForSeconds(1);
            InstantiateShoot();
        }
    }

    public void InstantiateShoot()
    {
        GameObject BulletClone = Instantiate(Bullet, transform.position, Quaternion.identity);
        BulletClone.GetComponent<Rigidbody2D>().velocity = new Vector3(targetPos.x, targetPos.y, 0) * BulletVelocity * Time.deltaTime;
        Debug.Log("shoot");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemy is In Range
        InRange = true;
        StartCoroutine("shootTimer");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Enemy is out of range 
        InRange = false;
        StopCoroutine("shootTimer");
    }
}
