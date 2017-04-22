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

    public void OnClick(GameObject upgradeObject)
    {
        Upgrade upgrade = upgradeObject.GetComponent<Upgrade>();

        // Check if it's available and we can afford it
        if (upgrade.isAvailableToUnlock() && upgrade.canAfford())
        {
            upgrade.grant();
        }
    }
}
