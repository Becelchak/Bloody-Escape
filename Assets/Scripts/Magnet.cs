using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Magnet : MonoBehaviour
{
    public float powerMagnet = 10f;
    private bool needMagnet;
    private GameObject objectMagnet;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (needMagnet)
        {
            var Xdif = objectMagnet.transform.position.x - transform.position.x;
            var Ydif = objectMagnet.transform.position.y - transform.position.y;
            var characterdirection = new Vector2(Xdif, Ydif);
            var bodyObject = objectMagnet.GetComponent<Rigidbody2D>();
            bodyObject.AddForce(-characterdirection.normalized * powerMagnet);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Front") return;
        needMagnet = true;
        objectMagnet = collision.gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Output magnet");
        needMagnet = false;
        if (objectMagnet != null) 
            objectMagnet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        objectMagnet = null;
    }

}
