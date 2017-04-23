using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {

	//public bool playerOwns = false;
	public List<Upgrade> dependencies = new List<Upgrade>();
	public string name = "";
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

	public void grant()
	{
        // Safety check so we don't add the same thing twice
		if (!(Game.thePlayer.hasUpgrade(this.name)))
        {
            Game.thePlayer.upgrades.Add(this);
            Game.thePlayer.XP = Game.thePlayer.XP - cost;
        }
	}
}
