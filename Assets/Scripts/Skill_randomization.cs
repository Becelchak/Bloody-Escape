using UnityEngine;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class Skill_randomization : MonoBehaviour
{
    // Skill parameter
    public int maxSkillOnLevel = 6;

    private int skillNumber;

    private string skillName;

    // UI component
    public Camera mainCamera;

    private Image skillImage;

    private GameObject dice;

    public float diceTime = 1.5f;

    // Audio for dice
    public AudioSource source = new AudioSource();
    public AudioClip diceRoll;

    void Start()
    {
        skillImage = mainCamera.transform.Find("Canvas/Skills/Skill panel/Skill").GetComponent<Image>();
        dice = mainCamera.transform.Find("Canvas/Skills/Dice").gameObject;

        // Get number skill
        skillNumber = Random.Range(3, 3);
        source.PlayOneShot(diceRoll);
        switch (skillNumber)
        {
            // Spikes
            case 1:
                dice.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skills/Dice/2");
                dice.GetComponent<CanvasGroup>().alpha = 1f;
                skillImage.sprite = Resources.Load<Sprite>("UI/Skills/Spikes");
                skillName = "Spikes";
                return;
            // Climbing
            case 2:
                dice.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skills/Dice/3");
                dice.GetComponent<CanvasGroup>().alpha = 1f;
                skillImage.sprite = Resources.Load<Sprite>("UI/Skills/Climbing");
                skillName = "Climbing";
                return;
            // Hook
            case 3:
                dice.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skills/Dice/1");
                dice.GetComponent<CanvasGroup>().alpha = 1f;
                skillImage.sprite = Resources.Load<Sprite>("UI/Skills/Hook");
                skillName = "Hook";
                return;
            // Whip
            case 4:
                dice.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skills/Dice/4");
                dice.GetComponent<CanvasGroup>().alpha = 1f;
                skillImage.sprite = Resources.Load<Sprite>("UI/Skills/Whip");
                skillName = "Whip";
                return;
            // Invulnerability
            case 5:
                dice.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skills/Dice/5");
                dice.GetComponent<CanvasGroup>().alpha = 1f;
                skillImage.sprite = Resources.Load<Sprite>("UI/Skills/Invulnerability");
                skillName = "Invulnerability";
                return;
            // Masking
            case 6:
                dice.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skills/Dice/6");
                dice.GetComponent<CanvasGroup>().alpha = 1f;
                skillImage.sprite = Resources.Load<Sprite>("UI/Skills/Masking");
                skillName = "Masking";
                return;
            default:
                return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (diceTime > 0)
            diceTime -= Time.deltaTime;
        if (diceTime <= 0)
            dice.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public string GetSpecialSkillName()
    {
        return skillName;
    }

    public Image GetSkillImage()
    {
        return skillImage;
    }
}
