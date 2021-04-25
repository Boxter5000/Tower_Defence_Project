using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("References")]
        [SerializeField] private CircleCollider2D RangeCollider;
        [SerializeField] private GameObject PlayerTargetPoint;
        
        [HideInInspector] private TowerInterface Interface;
        [HideInInspector] private Transform target;                 //Reference to the target to shoot
        [HideInInspector] private Vector3 targetPos;                //Position of that Target
        [HideInInspector] private Vector3 thisPos;                  //Position of this Tower
        [HideInInspector] private bool InRange = true;
        [HideInInspector] private Vector2 PlayerPickTargetPos;

    [Header("Tower Specs")]
        [SerializeField] private float BulletVelocity;
        [SerializeField] private float shootingRate;
        [SerializeField] private float Damage;
        [SerializeField] private float Range;
        [SerializeField] private float RotationalOffset;
        [SerializeField] public Bullet1 Ammo;
        [SerializeField] public Aim AimType;
        [SerializeField] private Behavior BehaviorType;

    //Other Information
        [HideInInspector] private float angle;
        [HideInInspector] private bool CR_runing = false;           //Is the Coroutine currently running?
        [HideInInspector] private bool IsShootingEnabled = true;    //Is the Tower allowed to shoot (for draging and placing Towers)
        [HideInInspector] List<GameObject> EnemysList;
        [HideInInspector] private bool PlayerTargetIsSet;
        [HideInInspector] private Vector3 TargetPoint;

    public enum Aim
    {
        Auto_Target,
        Player_Target,
        In_Range
    }
    public enum Behavior
    {
        Shot,
        Mörser,
        Nova
    }
    private void Awake()
    {
        Vector3 TargetPoint = Input.mousePosition;
        EnemysList = new List<GameObject>();
        RangeCollider.radius = Range;
        
        Interface = GameObject.FindGameObjectWithTag("UI").transform.Find("TowerInterface").GetComponent<TowerInterface>();
    }
    IEnumerator shootTimer()
    {
        CR_runing = true;
        if (IsShootingEnabled && CR_runing)
        {
            while(EnemysList.Count != 0 || AimType == Aim.Player_Target)
            {
                
                GetTargetType();
                yield return new WaitForSeconds(shootingRate);
            }
            CR_runing = false;
        }
    }
    public void GetTargetType()
    {
        switch (AimType)
        {
            case Aim.Auto_Target:
                AimType_AutoTarget();
                break;
            case Aim.In_Range:
                AimType_InRange();
                break;
            case Aim.Player_Target:
                AimType_PlayerTarget();
                break;
        }
        switch (BehaviorType)
        {
            case Behavior.Shot:
                InstantiateShoot();
                break;
            case Behavior.Mörser:
                InstantiateMörser();
                break;
            case Behavior.Nova:
                InstantiateNova();
                break;
        }
    }
    public void AimType_AutoTarget()
    {
        if (IsShootingEnabled)
        {
            targetPos = EnemysList[0].transform.position;

            targetPos -= thisPos;

            //calkulate the radius and the angel of the rotation
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + RotationalOffset));

            if (targetPos != null)
                Debug.DrawLine(thisPos, targetPos);
        }
    }
    public void AimType_PlayerTarget()
    {
        targetPos = PlayerPickTargetPos;
        targetPos -= thisPos;
    }
    public void AimType_InRange()
    {
        targetPos = Vector3.zero;
    }
    public void InstantiateShoot()
    {
        Vector3 VelocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 BulletClone = Instantiate(Ammo, transform.position, Quaternion.identity);

        BulletClone.Damage = Damage;
        BulletClone.GiveDirection(VelocityDirection, BulletVelocity);
    }
    public void InstantiateMörser()
    {
        Vector3 VelocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 BulletClone = Instantiate(Ammo, transform.position, Quaternion.identity);

        BulletClone.Damage = Damage;
        BulletClone.GiveDirection(VelocityDirection, BulletVelocity);
    }
    public void InstantiateNova()
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
            EnemysList.Add(collision.gameObject);   

            if(!CR_runing)
            {
                StartCoroutine("shootTimer");
            }
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
    public void GetPlacePos()
    {
        thisPos = transform.position;
        if (AimType == Aim.Player_Target)
        {
            Instantiate(PlayerTargetPoint, transform.position, quaternion.identity, transform);
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
    public void SetPlayerPosition(Vector2 PlayerPos)
    {
        PlayerPickTargetPos = PlayerPos;
        //manage this later with Master Spawner
        if(!CR_runing)
        {
            StartCoroutine("shootTimer");
        }
    }
    public void OnMouseDown()
    {
        Interface.gameObject.SetActive(true);
        Interface.GetReferenceTower(this);
    }
}
