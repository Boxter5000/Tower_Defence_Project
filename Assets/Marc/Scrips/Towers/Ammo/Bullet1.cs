using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float Countdown;
    Vector3 Direction = Vector3.zero;
    public float BulletVelocity = 0f;
    public float Damage = 10f;

    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = Direction.normalized * BulletVelocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public void GiveDirection(Vector3 Direction, float bulletSpeed)
    {
        this.Direction = Direction;
        this.BulletVelocity = bulletSpeed;
    }
}
