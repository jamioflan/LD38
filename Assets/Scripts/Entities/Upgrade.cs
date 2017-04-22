using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {

	//public bool playerOwns = false;
	public List<Upgrade> dependencies = new List<Upgrade>();
	public string description = "";
	public int cost = 100;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool isAvailable ()
	{
		bool has = true;
		foreach(Upgrade upgrade in dependencies)
		{
			if(Game.thePlayer.upgrades.Contains(upgrade))
			{
				has = has && true;
			}
			else
			{
				has = false;
			}
		}
		return has;
	}

	public bool canAfford ()
	{
		return Game.thePlayer.XP >= cost;
	}

	public void grant ()
	{
		Game.thePlayer.upgrades.Add(this);
		Game.thePlayer.XP = Game.thePlayer.XP - cost;
	}
}
