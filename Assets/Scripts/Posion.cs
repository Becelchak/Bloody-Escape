using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posion : MonoBehaviour
{
    public float timePosion = 5f;
    public float damageTick = 2f;
    public float damagePosion = 0.2f;
    private GameObject objectPosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        objectPosion = collision.gameObject;
        if (objectPosion.tag == "Player")
        {
            var player = objectPosion.GetComponent<PlayerControl>();
            if (player.GetStatus()) return;
            player.GetIntoxicated(timePosion, damageTick, damagePosion);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        objectPosion = collision.gameObject;
        if (objectPosion.tag == "Player")
        {
            var player = objectPosion.GetComponent<PlayerControl>();
            if (player.GetStatus()) return;
            player.GetIntoxicated(timePosion, damageTick, damagePosion);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        objectPosion = null;
    }
}
