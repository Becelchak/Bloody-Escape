using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commando : Enemy_parameter
{
    [Header("Attack")]
    [SerializeField] private CommandoTarget target;
    [SerializeField] private GameObject grenade;

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
        target.gameObject.SetActive(true);
        target.Navigate(transform.position);
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(target.chasingTime);
        var newGrenade = Instantiate(grenade, transform.position, transform.rotation).GetComponent<Grenade>();
        newGrenade.Shoot(Vector3.Normalize(target.transform.position - transform.position), target);
    }
}
