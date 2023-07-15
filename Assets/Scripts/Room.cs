using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int enemiesCount;
    public GameObject[] doors;
    public EnemyTypeRandomizing[] randomizers;

    public Skill_randomization skill;
    private bool isVisited = false;

    public void ClearFromEnemy()
    {
        enemiesCount--;

        if(enemiesCount == 0 && doors.Length > 0)
        {
            foreach(var door in doors)
                door.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isVisited)
        {
            isVisited = true;
            skill.Dice();
            foreach (var randomer in randomizers)
                randomer.Randomize();
        }
    }
}
