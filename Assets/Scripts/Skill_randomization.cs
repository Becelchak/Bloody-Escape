using UnityEngine;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;

public class Skill_randomization : MonoBehaviour
{
    // Skill parameter
    public int maxSkillOnLevel = 6;

    private int skillNumber;

    private string skillName;

    // UI component
    public Camera mainCamera;

    public Image skillImage;

    private GameObject dice;

    public float diceTime = 1.5f;

    // Audio for dice
    public AudioSource source = new AudioSource();
    public AudioClip diceRoll;

    public Animator diceAnimator;
    public float animationTime;
    private float animationTimer;
    public bool start;

    public Sprite hook, spikes, climbing, whip, invincibility, mask;

    // Update is called once per frame
    private void Update()
    {
        if(start)
        {
            if(animationTimer <= 0)
            {
                Player_Control.isAbleToMove = true;
                diceAnimator.SetBool("start", false);
                start = false;
                ShowAttack();
            }
            else
                animationTimer -= Time.deltaTime;
        }
    }

    public string GetSpecialSkillName()
    {
        return skillName;
    }

    public Image GetSkillImage()
    {
        return skillImage;
    }

    public void Dice()
    {
        skillNumber = Random.Range(1, maxSkillOnLevel + 1);
        diceAnimator.SetFloat("Attack", skillNumber);
        diceAnimator.SetBool("start", true);
        animationTimer = animationTime;
        Player_Control.isAbleToMove = false;
        start = true;
    }

    private void ShowAttack()
    {
        switch (skillNumber)
        {
            // Spikes
            case 1:
                skillImage.sprite = spikes;
                skillName = "Spikes";
                break;
            // Climbing
            case 2:
                skillImage.sprite = climbing;
                skillName = "Climbing";
                break;
            // Hook
            case 3:
                skillImage.sprite = hook;
                skillName = "Hook";
                break;
            // Whip
            case 4:
                skillImage.sprite = whip;
                skillName = "Whip";
                break;
            // Invulnerability
            case 5:
                skillImage.sprite = invincibility;
                skillName = "Invulnerability";
                break;
            // Masking
            case 6:
                skillImage.sprite = mask;
                skillName = "Masking";
                break;
            default:
                break;
        }
    }
}
