using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Manager : MonoBehaviour
{
    // The names of the GUI objects we're editing
    public string amountXPElementName;
    public string playerHealthElementName;
    public string equippedWeaponIconElementName;

    // Equipped weapon icons to use
    public Sprite equippedMeleeSprite;
    public Sprite equippedRangedSprite;
    public Sprite equippedMagicSprite;

    // The GUI objects we're editing
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
