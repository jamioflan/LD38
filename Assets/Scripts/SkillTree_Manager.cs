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

    public void OnClick( string upgradeObjectName )
    {
        GameObject upgradeObject = GameObject.Find(upgradeObjectName);

        // Check if it's available and we can afford it


        // Do stuff with the attached script/function
    }
}
