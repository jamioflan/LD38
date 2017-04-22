using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Manager : MonoBehaviour
{
    public string amountXPElementName;
    public string playerHealthElementName;
    public string equippedWeaponIconElementName;

    GUIText amountXPElement;
    

	// Use this for initialization
	void Start ()
    {
		amountXPElement = GameObject.Find(amountXPElementName).GetComponent<GUIText>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }
}
