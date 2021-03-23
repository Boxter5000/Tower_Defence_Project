using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public string Tag;
    public float offset;

    public Bullet1 Bullet;
    public float BulletVelocity;
    public float shootingRate;

    private Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;
    private bool CR_runing;

    List<GameObject> EnemysList;

    private bool InRange = true;

    private void Start()
    {
        EnemysList = new List<GameObject>();
    }


    public void GetTargetPos()
    {
        //get the positions of Tower and Enemy
        targetPos = EnemysList[0].transform.position;
        thisPos = transform.position;

        //get distans betwene tower and Enemy
        targetPos = targetPos - thisPos;

        //calkulate the radius and the angel of the rotation
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

        //rotate the tower
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        if (target != null)
        {
            Debug.DrawLine(thisPos, target.position);
        }
    }
    
    //timer to controll the shoot rate
    IEnumerator shootTimer()
    {
        CR_runing = true;
        //while something is in range, shoot
        while(EnemysList.Count != 0)
        {
            GetTargetPos();
            InstantiateShoot();
            //cerate bullet
            yield return new WaitForSeconds(shootingRate);

        }
        CR_runing = false;
    }

    public void InstantiateShoot()
    {
        //set direktion in witch the bullet needts to shoot
        Vector3 VelocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        //create a bullet
        Bullet1 BulletClone = Instantiate(Bullet, transform.position, Quaternion.identity);
        //set the velocity of the bullet
        BulletClone.GiveDirection(VelocityDirection, BulletVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currentEnemy = collision.gameObject;
        EnemysList.Add(currentEnemy);
        //get the possition of the collidet enemy
        target = collision.transform;
        //Enemy is In Range
        if(!CR_runing)
        {
            StartCoroutine("shootTimer");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject currentEnemy = collision.gameObject;
        EnemysList.Remove(currentEnemy);
        //Enemy is out of range 
        if(EnemysList.Count == 0)
        {
            CR_runing = false;
        }
    }
}
