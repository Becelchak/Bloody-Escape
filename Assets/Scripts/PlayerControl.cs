using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Parameter
    private static float bioMassNow = 1;
    private static float bioMassMin = 1;
    private static float bioMassMax = 2;

    // Effect parameter
    private bool intoxicated;
    private float damageEffect;
    private int countDamage;


    // Move
    public float speed = 2.0f;
    private Rigidbody2D controller;

    // Fight
    private BoxCollider2D front;
    private GameObject enemy;
    private bool eableEatEnemy;
    private static bool immortal;


    // Other
    public Camera playerCamera;
    //private float timer;

    void Start()
    {
        controller = GetComponent<Rigidbody2D>();
        front = GetComponentInChildren<BoxCollider2D>();
        //timer = 100f;
    }

    void Update()
    {
        if (bioMassNow == 0)
        {
            Destroy(this);
        }
        // Eat enemy on left button mouse
        if (Input.GetMouseButtonDown(0) && eableEatEnemy)
        {
            Destroy(enemy);
            BiomassUp(0.2f);
            //Debug.Log("Eating start");
            //while (timer > 0)
            //{
            //    timer -= Time.deltaTime;
            //    Debug.Log(timer);
            //    if (timer <= 0)
            //    {
            //        Debug.Log("Eating end");
            //        Destroy(enemy);
            //        BiomassUp(0.2f);
            //    }
            //}
            //timer = 100f;
        }
        if (!intoxicated) return;
        if (countDamage > 0)
        {
            BiomassDown(damageEffect);
            countDamage--;
            return;
        }
        UnIntoxicated();
    }

    // Move player
    void FixedUpdate()
    {

        // Follow mouse
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Move player
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        controller.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);

    }

    // Enemy in attack radius player
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Input player front");
        if (collision.gameObject.tag == "Enemy")
            enemy = collision.gameObject;
        eableEatEnemy = enemy != null;
    }

    // Enemy out attack radius player
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Output player front");
        enemy = null;
        eableEatEnemy = false;
    }

    // Any heal for player
    private static void BiomassUp(float massMultiplier)
    {
        bioMassNow = bioMassNow + bioMassNow * massMultiplier > bioMassMax ? bioMassMax 
            : bioMassNow + bioMassNow * massMultiplier;
    }

    // Any damage for player
    private static void BiomassDown(float massMultiplier)
    {
        if (immortal) return;
        bioMassNow = bioMassNow - bioMassMin * massMultiplier < bioMassMin ? 0
            : bioMassNow - bioMassMin * massMultiplier;
    }

    public float ShowBiomassNow()
    {
        return bioMassNow;
    }
    
    // Get intoxicated from acid
    public void GetIntoxicated(int allTime, int damageTick, float damageMultiplier)
    {
        intoxicated = true;
        countDamage = allTime / damageTick;
        damageEffect = damageMultiplier;
        var iconPosion = playerCamera.transform.Find("Canvas/PosionEffect");
        iconPosion.GetComponent<CanvasGroup>().alpha = 1f;
    }

    private void UnIntoxicated()
    {
        intoxicated = false;
        immortal = true;
        var iconPosion = playerCamera.transform.Find("Canvas/PosionEffect");
        iconPosion.GetComponent<CanvasGroup>().alpha = 0f;

    }
    public bool GetStatus()
    {
        return intoxicated;
    }
}
