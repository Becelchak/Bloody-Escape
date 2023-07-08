using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;
    public GameObject player;
    private float playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = player.GetComponent<PlayerControl>().ShowBiomassNow();
        slider.value = playerHealth;
    }
}
