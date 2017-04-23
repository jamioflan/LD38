using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree_Manager : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public static void OnClick(Upgrade upgrade)
    {
        // Check if it's available and we can afford it
        if (upgrade.isAvailableToUnlock() && upgrade.canAfford())
        {
            upgrade.grant();
        }
    }
}
