using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeRandomizing : MonoBehaviour
{
    [SerializeField] private Enemy_parameter[] availableEnemies;

    void Start()
    {
        Randomize();
    }

    void Update()
    {
        
    }

    public void Randomize()
    {
        if (availableEnemies.Length > 0)
        {
            availableEnemies[Random.Range(0, availableEnemies.Length)].gameObject.SetActive(true);
        }
    }
}
