using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeRandomizing : MonoBehaviour
{
    [SerializeField] private Enemy_parameter[] availableEnemies;
    [SerializeField] private Animator dice;
    [SerializeField] private float diceTime;
    [SerializeField] private Transform downPos;
    private float timer;
    private int enemyNumber;
    private bool start;

    void Start()
    {
        timer = diceTime;
    }

    void Update()
    {
        if (start)
        {
            if (timer <= 0)
            {
                availableEnemies[enemyNumber].gameObject.SetActive(true);
                dice.speed = 0;
                dice.gameObject.SetActive(false);
                start = false;
            }
            else
                timer -= Time.deltaTime;
        }
    }

    public void Randomize()
    {
        start = true;
        enemyNumber = Random.Range(0, availableEnemies.Length);
        dice.transform.localPosition = downPos.localPosition;
        dice.gameObject.SetActive(true);
        dice.SetFloat("EnemyNumber", enemyNumber);
        dice.speed = 1;
        timer = diceTime;
    }
}
