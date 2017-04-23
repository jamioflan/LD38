using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEye : Enemy
{
    public GameObject eyeClosed, eyeOpen, eyeDead;
    public bool isOpen = false;
    public float swapThreshold1, swapThreshold2;
    public Boss parent;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        eyeDead.SetActive(false);
        eyeOpen.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Damage(Attack attack)
    {
        if (!isOpen)
        {
            // Play sound
            return;
        }

        float preAttackHP = health;
        base.Damage(attack);
        float postAttackHP = health;

        if (preAttackHP > swapThreshold1 && postAttackHP < swapThreshold1)
        {
            parent.SwapEyes();
        }

        if (preAttackHP > swapThreshold2 && postAttackHP < swapThreshold2)
        {
            parent.SwapEyes();
        }
    }

    public void Deactivate()
    {
        eyeOpen.SetActive(false);
        eyeClosed.SetActive(true);
        isOpen = false;
    }

    public void Activate()
    {
        eyeOpen.SetActive(true);
        eyeClosed.SetActive(false);
        isOpen = true;
    }

    public override void Die()
    {
        eyeOpen.SetActive(false);
        eyeClosed.SetActive(false);
        eyeDead.SetActive(true);

        Game.thePlayer.XP += XP;
    }
}