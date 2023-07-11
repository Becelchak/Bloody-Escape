using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posion : MonoBehaviour
{
    public float timePosion = 5f;
    public float damageTick = 2f;
    public float damagePosion = 0.2f;
    private GameObject objectPosion;

    void OnTriggerEnter2D(Collider2D collision)
    {
        objectPosion = collision.gameObject;
        // If player enter in trigger zone -> set status intoxicated
        if (objectPosion.tag == "Player")
        {
            var player = objectPosion.GetComponent<Player_Control>();
            if (player.GetIntoxicatedStatus() || player.GetImmortalStatus()) return;
            player.GetIntoxicated(timePosion, damageTick, damagePosion);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        objectPosion = collision.gameObject;
        // If player stay in trigger zone -> set status intoxicated
        if (objectPosion.tag == "Player")
        {
            var player = objectPosion.GetComponent<Player_Control>();
            if (player.GetIntoxicatedStatus() || player.GetImmortalStatus()) return;
            player.GetIntoxicated(timePosion, damageTick, damagePosion);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        objectPosion = null;
    }
}
