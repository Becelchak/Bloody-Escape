using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float speed;
    private Rigidbody2D rigidBody;

    [Header("Damage")]
    [SerializeField] private float damage;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 direction)
    {
        rigidBody.AddForce(speed * direction, ForceMode2D.Force);
        rigidBody.velocity = speed * direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Walls")
        {
            if(collision.tag == "Player")
                Player_Control.BiomassDown(damage);
            Destroy(gameObject);
        }
    }
}
