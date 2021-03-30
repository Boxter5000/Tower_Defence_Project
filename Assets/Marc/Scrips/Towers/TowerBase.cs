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
    public float Range;

    private Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;
    private bool CR_runing;
    private CircleCollider2D CC2d;

    List<GameObject> EnemysList;

    private bool InRange = true;

    private void Awake()
    {
        EnemysList = new List<GameObject>();
        CC2d = GetComponent<CircleCollider2D>();
        CC2d.radius = Range;
    }

    public void GetTargetPos()
    {
        targetPos = EnemysList[0].transform.position;
        thisPos = transform.position;

        targetPos = targetPos - thisPos;
        //calkulate the radius and the angel of the rotation
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        if (target != null)
        {
            Debug.DrawLine(thisPos, target.position);
        }
    }
    
    IEnumerator shootTimer()
    {
        CR_runing = true;
        //while something is in range, shoot
        while(EnemysList.Count != 0)
        {
            GetTargetPos();
            InstantiateShoot();
            yield return new WaitForSeconds(shootingRate);
        }
        CR_runing = false;
    }

    public void InstantiateShoot()
    {
        Vector3 VelocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 BulletClone = Instantiate(Bullet, transform.position, Quaternion.identity);
        BulletClone.GiveDirection(VelocityDirection, BulletVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currentEnemy = collision.gameObject;
        EnemysList.Add(currentEnemy);
        //get the possition of the collidet enemy
        target = collision.transform;
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
