using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Сonveyor : MonoBehaviour
{
    public float speedRotate = 2f;

    public string direction;

    private float changeRotationTime = 5f;

    private bool timeChangeSpeed;

    private bool isObjectContain;

    private GameObject ObjectForMove;

    void Start()
    {
        
    }

    void Update()
    {
        changeRotationTime -= Time.deltaTime;
        if (changeRotationTime < 0)
        {
            changeRotationTime = 5f;
            timeChangeSpeed = true;
        }

        if (timeChangeSpeed)
        {
            timeChangeSpeed = false;
            //speedRotate = (float)Math.Ceiling(Random.Range(2f, 4f));
        }

        if(isObjectContain && ObjectForMove.GetComponent<Rigidbody2D>() != null)
        {
            switch (direction)
            {
                case "Right":
                    var Xdifright = ObjectForMove.transform.position.x + transform.position.x;
                    var rightDirection = new Vector2(Xdifright, 0);
                    ObjectForMove.GetComponent<Rigidbody2D>().AddForce(-rightDirection.normalized * speedRotate);
                    return;
                case "Left":
                    var XdifLeft = ObjectForMove.transform.position.x + transform.position.x;
                    var leftDirection = new Vector2(XdifLeft, 0);
                    ObjectForMove.GetComponent<Rigidbody2D>().AddForce(leftDirection.normalized * speedRotate);
                    return;
                case "Up":
                    var YdifUp = ObjectForMove.transform.position.y + transform.position.y;
                    var upDirection = new Vector2(0, YdifUp);
                    ObjectForMove.GetComponent<Rigidbody2D>().AddForce(upDirection.normalized * speedRotate);
                    return;
                case "Down":
                    var YdifDown = ObjectForMove.transform.position.y + transform.position.y;
                    var downDirection = new Vector2(0, YdifDown);
                    ObjectForMove.GetComponent<Rigidbody2D>().AddForce(-downDirection.normalized * speedRotate);
                    return;
                default:
                    return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Front") return;
        ObjectForMove = collision.gameObject;
        isObjectContain = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Front") return;
        ObjectForMove = collision.gameObject;
        isObjectContain = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (ObjectForMove != null)
            ObjectForMove = null;
        isObjectContain = false;
    }
}
