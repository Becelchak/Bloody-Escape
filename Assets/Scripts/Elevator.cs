using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform nextPosition;
    [SerializeField] private bool isFinal;
    [SerializeField] private Room room;
    [SerializeField] GameObject text;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Return) && room.enemiesCount == 0)
            {
                text.SetActive(false);
                if (isFinal)
                    collision.GetComponent<Player_Control>().FinishGame();
                else
                    Player_Control.SetPosition(nextPosition.position);
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.SetActive(false);
        }
    }
}
