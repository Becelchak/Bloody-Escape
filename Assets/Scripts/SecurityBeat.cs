using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityBeat : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float activeTime;
    private float activeTimer;

    private void Start()
    {
        activeTimer = activeTime;
    }

    private void Update()
    {
        if(activeTimer <= 0)
        {
            activeTimer = activeTime;
            gameObject.SetActive(false);
        }
        else
            activeTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player_Control.BiomassDown(damage);
            gameObject.SetActive(false);
        }
    }
}
