using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Transform = UnityEngine.Transform;

public class Player_Control : MonoBehaviour
{
    // Parameter
    private static float bioMassNow = 1;
    private static float bioMassMin = 1;
    private static float bioMassMax = 2;
    public static bool isInvisible { get; private set; }

    // Effect parameter from acid
    public Image iconPosion;
    private bool intoxicated;
    private float damageEffect;
    private float intoxicatedTime;
    private float damageTick;
    // Temp parameter acid
    private float tempTick;
    private float tempAlltime;
    // Effect immortal
    public Image iconImmortal;
    private float tempAllImmortaltime;


    // Move
    private static float speed = 8.0f;
    private Rigidbody2D controller;

    // Fight
    public GameObject enemy;
    public bool eableEatEnemy;
    private static bool immortal;
    public Image imageMainSkill;
    private Skill_randomization specialSkillName;
    private Image imageSpecialSkill;


    // Other
    public Camera playerCamera;
    private bool isMenuOpen;
    private CanvasGroup menuNow;
    public static bool isDead { get; private set; }
    private static SpriteRenderer render;
    private static Transform transform1;
    private static CanvasGroup defeatMenu;
    public CanvasGroup dm;
    public CanvasGroup winMenu;

    // Cooldown 1f = 1 second
    // Skill
    private float cooldownTime = 2f;
    private bool isCooldown;
    // Special skill
    private bool isSpecialCoolDown;
    private float specialCoolDownTime = 30f;
    // Special skill climbing
    private float stealthCoolDownTime = 10f;
    // Immortal
    private float coolDownImmortalTime = 0.8f;
    private float cooldDownInvulnerabilityTime = 7f;
    //Hiding
    public static bool isHiding { get; private set; }

    public static bool isAbleToMove { get; set; }

    [SerializeField] private Hook_logic hook;
    [SerializeField] private Spike_Zone spikesZone;
    [SerializeField] private Whipe_zone whipe;

    void Start()
    {
        //Initialization player parameter
        speed = 8f;
        controller = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        transform1 = GetComponent<Transform>();
        defeatMenu = dm;
        specialSkillName = GetComponent<Skill_randomization>();
        imageSpecialSkill = GetComponent<Skill_randomization>().GetSkillImage();
        // If level restart
        isDead = false;
    }

    void Update()
    {
        // Cooldown intoxicated effect
        IntoxicatedStatus();

        // Immortal cooldown after damage or effect
        ImmortalStatus();

        // Stealth time for Climbing and Masking skilles
        StealthStatus();

        // Main Attack logic
        MainAttack();

        // If player dead -> block control
        if (isDead) return;

        //Test mechanic of hiding
        //Hide();

        // Drop some biomass
        DropdownBiomass();

        //Menu open
        if (Input.GetKeyDown(KeyCode.Escape))
            ChangeMenuStatus();
        if (isMenuOpen)
            MenuStatus();
        else
            MenuClose();

        if (isMenuOpen) return;

        // Special Attack logic
        SpecialAttack(specialSkillName.GetSpecialSkillName());

        // Drop some biomass
        DropdownBiomass();

        // If player use shift -> up speed on 1,5
        ShiftStatus();
    }
    
    // Move player
    void FixedUpdate()
    {
        // If player dead -> block control
        if (isDead || isMenuOpen) return;

        // Follow mouse
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform1.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform1.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Move player
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        controller.velocity = isAbleToMove ? new Vector2(moveHorizontal * speed, moveVertical * speed) : Vector2.zero;

    }

    private static void DeadPlayer()
    {
        isDead = true;
        render.color = Color.black;
        defeatMenu.alpha = 1f;
        defeatMenu.blocksRaycasts = true;
        defeatMenu.interactable = true;
    }

    public void FinishGame()
    {
        winMenu.alpha = 1f;
        winMenu.blocksRaycasts = true;
        winMenu.interactable = true;
    }

    public void ChangeMenuStatus()
    {
        isMenuOpen = !isMenuOpen;
    }

    public void ChangeMenuNow(CanvasGroup menu)
    {
        menuNow = menu;
    }

    private void MenuClose()
    {
        menuNow.alpha = 0;
        menuNow.blocksRaycasts = false;
        menuNow.interactable = false;
    }

    private void MenuStatus()
    {
        menuNow.alpha = 1f;
        menuNow.blocksRaycasts = true;
        menuNow.interactable = true;
    }

    // See immortal status in player parameter
    private void ImmortalStatus()
    {
        if (!immortal) return;
        // Another immortal time for Invulnerability skill
        if (specialSkillName.GetSpecialSkillName() == "Invulnerability")
        {
            iconImmortal.fillAmount -= 1 / tempAllImmortaltime * Time.deltaTime;
            cooldDownInvulnerabilityTime -= Time.deltaTime;
            if (cooldDownInvulnerabilityTime < 0)
            {
                immortal = false;
                cooldDownInvulnerabilityTime = 7f;
            }
            return;
        }
        coolDownImmortalTime -= Time.deltaTime;
        iconImmortal.fillAmount -= 1 / tempAllImmortaltime * Time.deltaTime;
        if (coolDownImmortalTime <= 0)
        {
            immortal = false;
            coolDownImmortalTime = 0.8f;
        }
    }

    // See intoxicated status in player parameter
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

    // See stealth status in player parameter
    private void StealthStatus()
    {
        if ((specialSkillName.GetSpecialSkillName() == "Climbing" || specialSkillName.GetSpecialSkillName() == "Masking") && isInvisible)
        {
            stealthCoolDownTime -= Time.deltaTime;
            if (stealthCoolDownTime <= 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.1f, 0.1f, 1);
                isInvisible = false;
                if(specialSkillName.GetSpecialSkillName() == "Climbing")
                    intoxicated = false;
                stealthCoolDownTime = 10f;
            }
        }
    }

    private void DropdownBiomass()
    {
        // Can't drop biomass if that kill player
        if (Input.GetKeyDown(KeyCode.Space) && bioMassNow - 0.1f > bioMassMin)
        {
            bioMassNow -= 0.1f;
            transform1.localScale -= new Vector3(0.05f, 0.1f, 0);
            speed += bioMassNow - 1f;
        }
    }

    // See shift status in player parameter
    private void ShiftStatus()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed *= 1.5f;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed /= 1.5f;
    }

    private void MainAttack()
    {
        // Eat enemy on left button mouse
        if (Input.GetMouseButtonDown(1) 
            && eableEatEnemy 
            && !isCooldown 
            && enemy.GetComponent<Enemy_parameter>().CanBeDevoured())
        {
            // Enemy kill
            enemy.GetComponent<Enemy_parameter>().Clear();
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
        if (Input.GetMouseButtonDown(0) && !isSpecialCoolDown)
        {
            isSpecialCoolDown = true;
            imageSpecialSkill.fillAmount = 0;
            // Skill effect
            switch (specialSkillName.GetSpecialSkillName())
            {
                // Cooldown special attack On
                case "Hook":
                    hook.ActiveHook();
                    hook.Update();
                    return;
                case "Spikes":
                    spikesZone.ActivateSpike();
                    return;
                case "Climbing":
                    isInvisible = true;
                    intoxicated = true;
                    GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,0.3f);
                    return;
                case "Whip":
                    whipe.ActivateWhipe();
                    return;
                case "Invulnerability":
                    immortal = true;
                    iconImmortal.fillAmount = 1f;
                    tempAllImmortaltime = cooldDownInvulnerabilityTime;
                    return;
                case "Masking":
                    isInvisible = true;
                    GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.5f, 0.9f, 1);
                    return;
                default:
                    return;
            }
        }
        if (isSpecialCoolDown)
        {
            imageSpecialSkill.fillAmount += 1 / specialCoolDownTime * Time.deltaTime;
            if (imageSpecialSkill.fillAmount >= 1)
            {
                imageSpecialSkill.fillAmount = 1;
                isSpecialCoolDown = false;
            }
        }

    }

    // Any heal for player
    public static void BiomassUp(float massMultiplier)
    {
        // Grows after eat enemy
        if (bioMassNow + bioMassNow * massMultiplier <= bioMassMax || bioMassNow < bioMassMax)
        {
            transform1.localScale += new Vector3(0.1f, 0.2f, 0);
            // Change speed after change size player
            speed -= bioMassNow * massMultiplier * 2;
        }
        bioMassNow = bioMassNow + bioMassNow * massMultiplier > bioMassMax ? bioMassMax 
            : bioMassNow + bioMassNow * massMultiplier;
        

    }

    // Any damage for player
    public static void BiomassDown(float massMultiplier)
    {
        // If immortal mod ON, player don't take any damage
        if (immortal) return;
        bioMassNow = bioMassNow - bioMassMin * massMultiplier < bioMassMin ? 0
            : bioMassNow - bioMassMin * massMultiplier;
        // If biomass = 0 -> player dead
        if (bioMassNow == 0)
        {
            DeadPlayer();
        }
        // Decreases after take damage
        transform1.localScale -= new Vector3(0.1f, 0.2f, 0);
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
        iconPosion.fillAmount = 1f;

    }

    private void UnIntoxicated()
    {
        intoxicated = false;
        immortal = true;
        tempAllImmortaltime = coolDownImmortalTime;
        iconImmortal.fillAmount = 1f;

    }

    // Give intoxicated other class in other gameobjects
    public bool GetIntoxicatedStatus()
    {
        return intoxicated;
    }

    public static Vector3 GetPosition()
    {
        return transform1.position;
    }

    // Give immortal other class in other gameobjects
    public bool GetImmortalStatus()
    {
        return immortal;
    }

    public bool GetInvisibleStatus()
    {
        return isInvisible;
    }

    private void Hide()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isHiding = !isHiding;
            isInvisible = isHiding;
            intoxicated = isHiding;
            immortal = isHiding;
            render.color = isHiding ? Color.clear : Color.red;
        }
    }

    public static float GetBiomassSize()
    {
        return transform1.localScale.y - 2;
    }

    public static void SetPosition(Vector3 newPos)
    {
        transform1.position = newPos;
    }
}
