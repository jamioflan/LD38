using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Manager : MonoBehaviour
{
    public static HUD_Manager instance;

    // The names of the GUI objects we're editing
    public string amountXPElementName;
    public string playerHealthElementName;
    public string equippedWeaponIconElementName;
    public GameObject openDoorPrompt;

    // Equipped weapon icons to use
    public Sprite equippedMeleeSprite;
    public Sprite equippedRangedSprite;
    public Sprite equippedMagicSprite;

    // The GUI objects we're editing
    Text amountXPText;
    Slider playerHealthSlider;
    Image equippedWeaponIcon;

	// Use this for initialization
	void Start ()
    {
        instance = this;

        // Find and store the things we'll be modifying
		amountXPText = GameObject.Find(amountXPElementName).GetComponent<Text>();
        playerHealthSlider = GameObject.Find(playerHealthElementName).GetComponent<Slider>();
        equippedWeaponIcon = GameObject.Find(equippedWeaponIconElementName).GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update player XP, health and equipped weapon icon
        amountXPText.text = Game.thePlayer.XP.ToString() + " XP";
        playerHealthSlider.value = Game.thePlayer.health / Game.thePlayer.maxHealth;
        
        Sprite spriteToUse = equippedMeleeSprite; // default to melee

        switch (Game.thePlayer.currentAttack)
        {
            case 1: // ranged
            {
                spriteToUse = equippedRangedSprite;
                break;
            }
            case 2: // magic
            {
                spriteToUse = equippedMagicSprite;
                break;
            }
            case 0: // melee
            default:
            {
                break;
            }
        }

        equippedWeaponIcon.sprite = spriteToUse;
    }

    public void ShowDoorPrompt( Vector3 position )
    {
        openDoorPrompt.transform.position = position;
        openDoorPrompt.SetActive(true);
    }

    public void HideDoorPrompt()
    {
        openDoorPrompt.SetActive(false);
    }
}
