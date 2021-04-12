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
    public float TowerDamage;
    [SerializeField] CircleCollider2D RangeCollider;

    private Transform target;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;
    private bool CR_runing;
    private bool IsShootingEnabled = true;

    List<GameObject> EnemysList;

    private bool InRange = true;

    private void Awake()
    {
        EnemysList = new List<GameObject>();
        RangeCollider.radius = Range;
    }

    public void GetTargetPos()
    {
        if (IsShootingEnabled)
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
    }
    
    IEnumerator shootTimer()
    {
        if (IsShootingEnabled)
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
    }

    public void InstantiateShoot()
    {
        Vector3 VelocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 BulletClone = Instantiate(Bullet, transform.position, Quaternion.identity);
        BulletClone.Damage = TowerDamage;
        BulletClone.GiveDirection(VelocityDirection, BulletVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject currentEnemy = collision.gameObject;
            EnemysList.Add(currentEnemy);   
        }
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

    public void ManageShooting(bool State)
    {
        IsShootingEnabled = State;
    }

    public void ManageRangeVisuals(bool State)
    {
        GameObject RangeVisual = transform.Find("RangeVisual").gameObject;

        RangeVisual.GetComponent<SpriteRenderer>().enabled = State;
        RangeVisual.transform.localScale = new Vector2(Range * 2, Range * 2);
    }
}
