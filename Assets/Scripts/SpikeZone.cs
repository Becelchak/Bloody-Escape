using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeZone : MonoBehaviour
{
    public GameObject spriteSpikes;
    private List<GameObject> Enemies = new List<GameObject>();
    private float spikesLiveTime = 5f;
    private bool isActive;
    private Vector3 freezePosition;
    void Start()
    {
    }

    void Update()
    {
        if (isActive)
        {
            spriteSpikes.gameObject.transform.position = freezePosition;
            spikesLiveTime -= Time.deltaTime;
            if (spikesLiveTime <= 0)
            {
                spikesLiveTime = 5f;
                isActive = false;
                spriteSpikes.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" 
            && !Enemies.Contains(collision.gameObject) 
            && collision.gameObject.GetComponent<Enemy_parameter>().EnemyAlive())
        {
            Enemies.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (Enemies.Contains(collision.gameObject))
            Enemies.Remove(collision.gameObject);
    }

    public void ActivateSpike()
    {
        freezePosition = transform.position;
        spriteSpikes.GetComponent<SpriteRenderer>().enabled = true;
        isActive = true;
        // Kill all enemy in spikes zone
        foreach (var enemy in Enemies)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.red;
            enemy.GetComponent<Enemy_parameter>().ChangeLiveStatus();
        }

        Enemies.Clear();
    }
}
