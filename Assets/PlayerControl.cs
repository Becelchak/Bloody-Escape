using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 2.0f;
    private Vector2 move = Vector3.zero;
    public float gravity = 10.0f;
    private Rigidbody2D controller;
    void Start()
    {
        controller = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        controller.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);

    }
}
