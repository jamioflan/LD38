using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Manager : MonoBehaviour
{
    public string amountXPElementName;
    public string playerHealthElementName;
    public string equippedWeaponIconElementName;

    GUIText amountXPText;
    Slider playerHealthSlider;
    Image equippedWeaponIcon;

	// Use this for initialization
	void Start ()
    {
        // Find and store the things we'll be modifying
		amountXPText = GameObject.Find(amountXPElementName).GetComponent<GUIText>();
        playerHealthSlider = GameObject.Find(playerHealthElementName).GetComponent<Slider>();
        equippedWeaponIcon = GameObject.Find(equippedWeaponIconElementName).GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update player XP, health and equipped weapon icon
        // TODO
        
    }
}
