using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("References")]
        [SerializeField] private CircleCollider2D RangeCollider;
        [SerializeField] private Bullet1 Ammo;

        [HideInInspector] private Transform target;                 //Reference to the target to shoot
        [HideInInspector] private Vector3 targetPos;                //Position of that Target
        [HideInInspector] private Vector3 thisPos;                  //Position of this Tower
        [HideInInspector] private bool InRange = true;

    [Header("Tower Specs")]
        [SerializeField] private float BulletVelocity;
        [SerializeField] private float shootingRate;
        [SerializeField] private float Damage;
        [SerializeField] private float Range;
        [SerializeField] private float RotationalOffset;

    //Other Information
        [HideInInspector] private float angle;
        [HideInInspector] private bool CR_runing;                   //Is the Coroutine currently running?
        [HideInInspector] private bool IsShootingEnabled = true;    //Is the Tower allowed to shoot (for draging and placing Towers)
        [HideInInspector] List<GameObject> EnemysList;

    private void Awake()
    {
        EnemysList = new List<GameObject>();
        RangeCollider.radius = Range;
        thisPos = transform.position;
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


    public void GetTargetPos()
    {
        if (IsShootingEnabled)
        {
            targetPos = EnemysList[0].transform.position;
            targetPos -= thisPos;

            //calkulate the radius and the angel of the rotation
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + RotationalOffset));

            if (target != null)
                Debug.DrawLine(thisPos, target.position);
        }
    }
    

    public void InstantiateShoot()
    {
        Vector3 VelocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 BulletClone = Instantiate(Ammo, transform.position, Quaternion.identity);
        BulletClone.Damage = Damage;
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
