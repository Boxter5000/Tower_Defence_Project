using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerBase : MonoBehaviour
{
    [Header("References")]
        [SerializeField] private CircleCollider2D rangeCollider;
        [SerializeField] private GameObject playerTargetPoint;
        
        [HideInInspector] private TowerInterface towerInterface;
        [HideInInspector] private Transform target;                 //Reference to the target to shoot
        [HideInInspector] private Vector3 targetPos;                //Position of that Target
        [HideInInspector] private Vector3 thisPos;                  //Position of this Tower
        [HideInInspector] private bool inRange = true;
        [HideInInspector] private Vector2 playerPickTargetPos;

    
    [Header("Tower Specs")]
        [SerializeField] private float bulletVelocity;
        [SerializeField] private float shootingRate;
        [SerializeField] private float damage;
        [SerializeField] private float range;
        [SerializeField] private float rotationalOffset;
        [SerializeField] public Bullet1 ammo;
        [SerializeField] public Aim aimType;
        [SerializeField] private Behavior behaviorType;

    //Other Information
        [HideInInspector] private float angle;
        [HideInInspector] private bool isCRRuning = false;           //Is the Coroutine currently running?
        [HideInInspector] private bool isShootingEnabled = true;    //Is the Tower allowed to shoot (for draging and placing Towers)
        [HideInInspector] List<GameObject> enemiesList;

    public enum Aim
    {
        AutoTarget,
        PlayerTarget,
        InRange
    }
    public enum Behavior
    {
        Shot,
        Moerser,
        Nova
    }
    private void Awake()
    {
        enemiesList = new List<GameObject>();
        rangeCollider.radius = range;
        
        towerInterface = GameObject.FindGameObjectWithTag("UI").transform.Find("TowerInterface").GetComponent<TowerInterface>();
    }
    private IEnumerator ShootTimer()
    {
        isCRRuning = true;
        if (isShootingEnabled && isCRRuning)
        {
            while(enemiesList.Count != 0 || aimType == Aim.PlayerTarget)
            {
                
                GetTargetType();
                yield return new WaitForSeconds(shootingRate);
            }
        }
    }

    private void GetTargetType()
    {
        switch (aimType)
        {
            case Aim.AutoTarget:
                AimType_AutoTarget();
                break;
            case Aim.InRange:
                AimType_InRange();
                break;
            case Aim.PlayerTarget:
                AimType_PlayerTarget();
                break;
        }
        switch (behaviorType)
        {
            case Behavior.Shot:
                InstantiateShoot();
                break;
            case Behavior.Moerser:
                InstantiateMörser();
                break;
            case Behavior.Nova:
                InstantiateNova();
                break;
        }
    }
    private void AimType_AutoTarget()
    {
        if (isShootingEnabled)
        {
            targetPos = enemiesList[0].transform.position;

            targetPos -= thisPos;

            //calkulate the radius and the angel of the rotation
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationalOffset));

            if (targetPos != null)
                Debug.DrawLine(thisPos, targetPos);
        }
    }
    private void AimType_PlayerTarget()
    {
        targetPos = playerPickTargetPos;
        targetPos -= thisPos;
    }
    private void AimType_InRange()
    {
        targetPos = Vector3.zero;
    }
    private void InstantiateShoot()
    {
        Vector3 velocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 bulletClone = Instantiate(ammo, transform.position, Quaternion.identity);

        bulletClone.Damage = damage;
        bulletClone.GiveDirection(velocityDirection, bulletVelocity);
    }
    private void InstantiateMörser()
    {
        Vector3 velocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 bulletClone = Instantiate(ammo, transform.position, Quaternion.identity);

        bulletClone.Damage = damage;
        bulletClone.GiveDirection(velocityDirection, bulletVelocity);
    }
    private void InstantiateNova()
    {
        Vector3 velocityDirection = new Vector3(targetPos.x, targetPos.y, 0).normalized;
        Bullet1 bulletClone = Instantiate(ammo, transform.position, Quaternion.identity);

        bulletClone.Damage = damage;
        bulletClone.GiveDirection(velocityDirection, bulletVelocity);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && aimType != Aim.PlayerTarget)
        {
            enemiesList.Add(collision.gameObject);   

            if(!isCRRuning)
            {
                StartCoroutine("ShootTimer");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject currentEnemy = collision.gameObject;
        enemiesList.Remove(currentEnemy);
        //Enemy is out of range 
        if(enemiesList.Count == 0 && aimType != Aim.PlayerTarget)
        {
            isCRRuning = false;
            StopCoroutine("ShootTimer");
        }
    }
    public void GetPlacePos()
    {
        thisPos = transform.position;
        if (aimType == Aim.PlayerTarget)
        {
            Instantiate(playerTargetPoint, transform.position, quaternion.identity, transform);
        }
    }
    public void ManageShooting(bool state)
    {
        isShootingEnabled = state;
    }
    public void ManageRangeVisuals(bool state)
    {
        GameObject rangeVisual = transform.Find("RangeVisual").gameObject;

        rangeVisual.GetComponent<SpriteRenderer>().enabled = state;
        rangeVisual.transform.localScale = new Vector2(range * 2, range * 2);
    }
    public void SetPlayerPosition(Vector2 playerPos)
    {
        playerPickTargetPos = playerPos;

        //calkulate the radius and the angel of the rotation
        angle = Mathf.Atan2(playerPickTargetPos.y - thisPos.y, playerPickTargetPos.x- thisPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationalOffset));
        
        //manage this later with Master Spawner
        if(!isCRRuning)
        {
            StartCoroutine("ShootTimer");
        }
    }
    public void OnMouseDown()
    {
        towerInterface.gameObject.SetActive(true);
        towerInterface.GetReferenceTower(this);
    }
}
