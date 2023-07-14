using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantRespawn : MonoBehaviour
{
    [SerializeField] private float respawnTime;
    private float timer;
    [SerializeField] Assistant assistant;

    void Start()
    {
        timer = respawnTime;
    }

    void Update()
    {
        if(!assistant.EnemyAlive())
        {
            if (timer <= 0)
            {
                assistant.gameObject.SetActive(true);
                assistant.ChangeLiveStatus();
                timer = respawnTime;
            }
            else
                timer -= Time.deltaTime;
        }
    }
}
