using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whipe_zone : MonoBehaviour
{

    public GameObject whipeSprite;
    private List<GameObject> Enemies = new List<GameObject>();
    private float WhipeAttackTime = 0.5f;
    private bool isActive;

    void Update()
    {
        if (isActive)
        {
            WhipeAttackTime -= Time.deltaTime;
            if (WhipeAttackTime <= 0)
            {
                WhipeAttackTime = 5f;
                isActive = false;
                whipeSprite.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy"
            && !Enemies.Contains(collision.gameObject)
            && collision.gameObject.GetComponent<Enemy_parameter>().EnemyAlive())
        {
            Enemies.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (Enemies.Contains(collision.gameObject))
            Enemies.Remove(collision.gameObject);
    }

    public void ActivateWhipe()
    {
        whipeSprite.GetComponent<SpriteRenderer>().enabled = true;
        isActive = true;
        // Kill all enemy in whipe zone
        foreach (var enemy in Enemies)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.red;
            enemy.GetComponent<Enemy_parameter>().ChangeLiveStatus();
        }

        Enemies.Clear();
    }
}
