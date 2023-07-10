using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Transform = UnityEngine.Transform;

public class PlayerControl : MonoBehaviour
{
    // Parameter
    private static float bioMassNow = 1;
    private static float bioMassMin = 1;
    private static float bioMassMax = 2;

    // Effect parameter from acid
    private Image iconPosion;
    private bool intoxicated;
    private float damageEffect;
    private float intoxicatedTime;
    private float damageTick;
    // Temp parameter acid
    private float tempTick;
    private float tempAlltime;


    // Move
    public static float speed = 8.0f;
    private Rigidbody2D controller;

    // Fight
    private BoxCollider2D front;
    private GameObject enemy;
    private bool eableEatEnemy;
    private static bool immortal;
    private Image imageMainSkill;
    private string specialSkillName;
    private Image imageSpecialSkill;


    // Other
    public Camera playerCamera;
    private bool isMenuOpen;
    private static bool isDead;
    private static SpriteRenderer render;
    private static Transform transform;
    private static CanvasGroup defeatMenu;

    // Cooldown
    // Skill
    private float cooldownTime = 2f;
    private bool isCooldown;
    // Special skill
    private bool isSpecialCooldown;
    private float specialCooldownTime = 2f;
    // Immortal
    private float coolDownImmortalTime = 0.8f;

    void Start()
    {
        //Initialization player parameter
        controller = GetComponent<Rigidbody2D>();
        front = GetComponentInChildren<BoxCollider2D>();
        imageMainSkill = playerCamera.transform.Find("Canvas/Attack main/Button").GetComponent<Image>();
        render = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        defeatMenu = playerCamera.transform.Find("Canvas/Menu/DefeatMenu").GetComponent<CanvasGroup>();
        specialSkillName = GetComponent<Skill_randomization>().GetSpecialSkillName();
        imageSpecialSkill = GetComponent<Skill_randomization>().GetSkillImage();
    }

    void Update()
    {
        // Main Attack logic
        MainAttack();

        // Special Attack logic
        SpecialAttack(specialSkillName);
        // Cooldown intoxicated effect
        IntoxicatedStatus();

        // Immortal cooldown after damage or effect
        ImmortalStatus();

        // If player dead -> block control
        if (isDead) return;

        //Menu open
        if (Input.GetKeyDown(KeyCode.Escape))
            ChangeMenuStatus();
        if (isMenuOpen)
            MenuStatus();
        else
            MenuClose();
    }
    
    // Move player
    void FixedUpdate()
    {
        // If player dead -> block control
        if (isDead) return;

        // Follow mouse
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Move player
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        controller.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);

    }

    private static void DeadPlayer()
    {
        isDead = true;
        render.color = Color.red;
        defeatMenu.alpha = 1f;
        defeatMenu.blocksRaycasts = true;
        defeatMenu.interactable = true;
    }

    public void ChangeMenuStatus()
    {
        isMenuOpen = !isMenuOpen;
    }

    private void MenuClose()
    {
        var menu = playerCamera.transform.Find("Canvas/Menu/MainMenu").GetComponent<CanvasGroup>();
        menu.alpha = 0;
        menu.blocksRaycasts = false;
        menu.interactable = false;
    }

    private void MenuStatus()
    {
        var menu = playerCamera.transform.Find("Canvas/Menu/MainMenu").GetComponent<CanvasGroup>();
        menu.alpha = 1f;
        menu.blocksRaycasts = true;
        menu.interactable = true;
    }

    private void ImmortalStatus()
    {
        if (!immortal) return;
        Debug.Log("Immortal");
        coolDownImmortalTime -= Time.deltaTime;
        if (coolDownImmortalTime <= 0)
        {
            immortal = false;
            coolDownImmortalTime = 0.8f;
            Debug.Log("Not Immortal");
        }
    }

    private void IntoxicatedStatus()
    {
        if (intoxicated && tempAlltime > 0)
        {
            iconPosion.fillAmount -= 1 / intoxicatedTime * Time.deltaTime;
            tempTick -= Time.deltaTime;
            tempAlltime -= Time.deltaTime;
            if(tempTick <= 0)
            {
                BiomassDown(damageEffect);
                tempTick = damageTick;
            }
            if(tempAlltime > 0) return;
            UnIntoxicated();
        }
    }

    private void MainAttack()
    {
        // Eat enemy on left button mouse
        if (Input.GetMouseButtonDown(1) && eableEatEnemy && !isCooldown)
        {
            // Enemy kill
            Destroy(enemy);
            BiomassUp(0.2f);

            // Cooldown attack On
            isCooldown = true;
            imageMainSkill.fillAmount = 0;
        }

        if (isCooldown)
        {
            imageMainSkill.fillAmount += 1 / cooldownTime * Time.deltaTime;
            if (imageMainSkill.fillAmount >= 1)
            {
                imageMainSkill.fillAmount = 1;
                isCooldown = false;
            }
        }
    }

    private void SpecialAttack(string nameAttack)
    {
        if (Input.GetMouseButtonDown(0) && !isSpecialCooldown)
        {
            // Skill effect
            switch (specialSkillName)
            {
                // Cooldown special attack On
                case "Hook":
                    isSpecialCooldown = true;
                    imageSpecialSkill.fillAmount = 0;
                    return;
                case "Spikes":
                    isSpecialCooldown = true;
                    imageSpecialSkill.fillAmount = 0;
                    return;
                case "Climbing":
                    isSpecialCooldown = true;
                    imageSpecialSkill.fillAmount = 0;
                    return;
                default:
                    return;
            }
        }
        if (isSpecialCooldown)
        {
            imageSpecialSkill.fillAmount += 1 / cooldownTime * Time.deltaTime;
            if (imageSpecialSkill.fillAmount >= 1)
            {
                imageSpecialSkill.fillAmount = 1;
                isSpecialCooldown = false;
            }
        }

    }

    // Enemy in attack radius player
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemy = collision.gameObject;
        eableEatEnemy = enemy != null;
    }

    // Enemy out attack radius player
    void OnTriggerExit2D(Collider2D collision)
    {
        enemy = null;
        eableEatEnemy = false;
    }

    // Any heal for player
    private static void BiomassUp(float massMultiplier)
    {
        // Grows after eat enemy
        if (bioMassNow + bioMassNow * massMultiplier <= bioMassMax || bioMassNow < bioMassMax)
        {
            transform.localScale += new Vector3(0.1f, 0.2f, 0);
            // Change speed after change size player
            speed -= bioMassNow * massMultiplier * 2;
        }
        bioMassNow = bioMassNow + bioMassNow * massMultiplier > bioMassMax ? bioMassMax 
            : bioMassNow + bioMassNow * massMultiplier;
        

    }

    // Any damage for player
    private static void BiomassDown(float massMultiplier)
    {
        // If immortal mod ON, player don't take any damage
        if (immortal) return;
        bioMassNow = bioMassNow - bioMassMin * massMultiplier < bioMassMin ? 0
            : bioMassNow - bioMassMin * massMultiplier;
        Debug.Log($"Take damage: {massMultiplier}");
        // If biomass = 0 -> player dead
        if (bioMassNow == 0)
        {
            DeadPlayer();
        }
        // Decreases after take damage
        transform.localScale -= new Vector3(0.1f, 0.2f, 0);
        // Change speed after change size player
        speed += (bioMassNow - 1f) * 2;
    }

    public float ShowBiomassNow()
    {
        return bioMassNow;
    }
    
    // Get intoxicated from posion
    public void GetIntoxicated(float allTime, float damageTick, float damageMultiplier)
    {
        // Get parameter intoxicated
        intoxicated = true;
        intoxicatedTime = allTime;
        tempAlltime = allTime;
        this.damageTick = damageTick;
        tempTick = damageTick;
        damageEffect = damageMultiplier;
        // Set icon for posion
        iconPosion = playerCamera.transform.Find("Canvas/PosionEffect/Button").GetComponent<Image>();
        iconPosion.fillAmount = 1f;

    }

    private void UnIntoxicated()
    {
        intoxicated = false;
        immortal = true;

    }
    public bool GetStatus()
    {
        return intoxicated;
    }
}
