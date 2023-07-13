using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage, activeTime;
    private float timer;
    private bool isExploding;

    private void Update()
    {
        if(isExploding)
        {
            if(timer > 0)
                timer -= Time.deltaTime;
            else
                Destroy(gameObject);
        }
    }

    public void Explode()
    {
        isExploding = true;
        timer = activeTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player_Control.BiomassDown(damage);
        }
    }
}
