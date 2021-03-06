﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {

	//public bool playerOwns = false;
	public List<Upgrade> dependencies = new List<Upgrade>();
	//public string name = "";
	public string description = "";
	public int cost = 100;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

	/*
	 * Hanging on to this in case we need it again. It's been replaced by the player.hasUpgrade() method

    public bool alreadyOwned()
    {
        if (Game.thePlayer.upgrades.Contains(this))
        {
            return true;
        }

        return false;
    }
    */

    // isAvailableToUnlock() returns false if we already own it
	public bool isAvailableToUnlock()
	{
		if (Game.thePlayer.hasUpgrade(this.name))
        {
            return false;
        }

		foreach(Upgrade upgrade in dependencies)
		{
			if(!Game.thePlayer.upgrades.Contains(upgrade))
			{
                // We don't have one of the dependencies - return false
                return false;
			}
		}

        // We've made it this far - either there are no dependencies, or we've got all of them
		return true;
	}

    public bool isOwnedOrAvailableToUnlock()
    {
		return (Game.thePlayer.hasUpgrade(this.name)) || isAvailableToUnlock();
    }

	public bool canAfford()
	{
		return Game.thePlayer.XP >= cost;
	}

	public void grant(Entity entity)
	{
        // Safety check so we don't add the same thing twice
		if (!(entity.hasUpgrade(this.name)))
        {
			entity.upgrades.Add(this);

			// If entity is the player, make them pay for it
			if (entity.isPlayer)
			{
				entity.XP = entity.XP - cost;
			}
			else
			{
				entity.XP = entity.XP + cost;
			}

			switch (this.name)
			{
				case "meleeIncreasedHealth":
					entity.maxHealth = 1.5F * entity.maxHealth;
					break;
				case "magicIncreasedRegeneration":
					entity.healthRegenRate = 2F * entity.healthRegenRate;
					break;
				case "rangedIncreasedSpeed":
					entity.moveSpeed = 1.5F * entity.moveSpeed;
					break;
			}
        }
	}
}
