using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook_eating_logic : MonoBehaviour
{
    public Player_Control player;

    // Catch enemy on hook
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player.enemy = collision.gameObject;
            player.eableEatEnemy = player.enemy != null;
            // Enemy kill
            collision.GetComponent<Enemy_parameter>().Clear();
            Destroy(collision.gameObject);
            Player_Control.BiomassUp(0.2f);
            Physics2D.IgnoreLayerCollision(8, 9, true);
        }
    }

    // Enemy out attack radius player
    void OnTriggerExit2D(Collider2D collision)
    {
        player.enemy = null;
        player.eableEatEnemy = false;
        Physics2D.IgnoreLayerCollision(8, 9, true);
    }
}
