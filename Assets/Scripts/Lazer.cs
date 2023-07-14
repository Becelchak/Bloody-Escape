using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player_Control.BiomassDown(damage);
        }
        else if(collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy_parameter>().ChangeLiveStatus();
            Destroy(collision.gameObject);
        }
    }
}
