using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Front_visible : MonoBehaviour
{
    private Player_Control player;
    
    void Start()
    {
        player = GetComponentInParent<Player_Control>();
    }

    // Enemy in attack radius player
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
            player.FinishGame();
        if (collision.gameObject.tag == "Enemy")
        {
            player.enemy = collision.gameObject;
            player.eableEatEnemy = player.enemy != null;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player.enemy = collision.gameObject;
            player.eableEatEnemy = player.enemy != null;
        }
    }

    // Enemy out attack radius player
    void OnTriggerExit2D(Collider2D collision)
    {
        player.enemy = null;
        player.eableEatEnemy = false;
    }
}
