using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public string Tag;
    public float offset;

    public GameObject Bullet;
    public float BulletVelocity;
    public float shootingRate;

    private Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;

    private bool InRange = false;

    void Update()
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

    //timer to controll the shoot rate
    IEnumerator shootTimer()
    {
        //while something is in range, shoot
        while(InRange)
        {
            yield return new WaitForSeconds(shootingRate);
            //cerate bullet
            InstantiateShoot();
        }
    }

    public void InstantiateShoot()
    {
        //set direktion in witch the bullet needts to shoot
        Vector3 VelovityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        //create a bullet
        GameObject BulletClone = Instantiate(Bullet, transform.position, Quaternion.identity);
        //set the velocity of the bullet
        BulletClone.GetComponent<Rigidbody2D>().velocity = VelovityDirection * BulletVelocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get the possition of the collidet enemy
        target = collision.transform;
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
