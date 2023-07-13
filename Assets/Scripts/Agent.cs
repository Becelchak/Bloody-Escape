using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Enemy_parameter
{
    [Header("Attack")]
    [SerializeField] private GameObject bullet;

    void Update()
    {
        HandleEnemyState();

        HandleAttack();
    }

    private void FixedUpdate()
    {
        MoveByState();
    }

    protected override void Attack()
    {
        //We could create a pool of bullet objects,
        //but it would be pretty hard and long, so I use Instantiate method
        var newBullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>();
        newBullet.Shoot(direction);
    }
}
