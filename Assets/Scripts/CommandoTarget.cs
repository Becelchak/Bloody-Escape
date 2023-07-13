using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandoTarget : MonoBehaviour
{
    public float chasingTime;
    private float chasingTimer;
    private bool isNavigating;

    void Update()
    {
        if(isNavigating)
        {
            if (chasingTimer > 0)
            {
                transform.position = Player_Control.GetPosition();
                chasingTimer -= Time.deltaTime;
            }
            else
            {
                isNavigating = false;
            }
        }
    }

    public void Navigate(Vector3 startPosition)
    {
        transform.position = startPosition;
        isNavigating = true;
        chasingTimer = chasingTime;
        transform.localScale = new Vector3(transform.localScale.x + Player_Control.GetBiomassSize(), transform.localScale.y + Player_Control.GetBiomassSize(), 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Grenade")
        {
            collision.GetComponent<Grenade>().Explode();
        }
    }
}
