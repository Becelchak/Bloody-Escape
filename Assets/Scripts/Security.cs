using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security : Enemy_parameter
{
    [Header("Attack")]
    [SerializeField] private GameObject beat;

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
        beat.SetActive(true);
    }
}
