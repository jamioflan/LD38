using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree_Manager : MonoBehaviour
{
    public GameObject amountXPObject;

    Text amountXPText;

	// Use this for initialization
	void Start ()
    {
        amountXPText = amountXPObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        amountXPText.text = Game.thePlayer.XP.ToString();
    }
}
