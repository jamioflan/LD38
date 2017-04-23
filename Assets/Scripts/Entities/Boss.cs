using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossEye[] eyes = new BossEye[2];
    public int currentEye = 0;

    // Use this for initialization
    public void Start()
    {
        ActivateBoss();
    }

    // Update is called once per frame
    public void Update()
    {

    }

    public void ActivateBoss()
    {
        SwapEyes();
    }

    public void SwapEyes()
    {
        eyes[currentEye].Deactivate();
        currentEye = 1 - currentEye;
        if (eyes[currentEye].health > 0)
        {
            eyes[currentEye].Activate();
        }
        else
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        // We need some sort of level flow here.
    }
}
