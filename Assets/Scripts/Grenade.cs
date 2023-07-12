using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float speed;
    private Rigidbody2D rigidBody;

    [Header("Damage")]
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosion;
    private CommandoTarget target;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 direction, CommandoTarget target)
    {
        rigidBody.AddForce(speed * direction, ForceMode2D.Force);
        rigidBody.velocity = speed * direction;
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Walls")
        {
            if (collision.tag == "Player")
                Player_Control.BiomassDown(damage);
            Explode();
        }
    }

    public void Explode()
    {
        var newExplosion = Instantiate(explosion, transform.position, transform.rotation).GetComponent<Explosion>();
        newExplosion.Explode();
        target?.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
