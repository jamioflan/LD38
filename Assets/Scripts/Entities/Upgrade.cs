using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {

	public bool playerOwns = false;
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
		return true;
	}
}
