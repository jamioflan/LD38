using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree_Manager : MonoBehaviour
{
    public GameObject amountXPObject;
    public GameObject tooltipObject;

    Text amountXPText;
    Text tooltipText;

    Color amountXPNormalColour;
    Color amountXPRedColour;

    float redXPTimer;
    public float redXPTime;

	// Use this for initialization
	void Start ()
    {
        Game.theSkillTreeManager = this;

        amountXPText = amountXPObject.GetComponent<Text>();
        tooltipText = tooltipObject.GetComponent<Text>();

        amountXPNormalColour = amountXPText.color;
        amountXPRedColour.r = 1.0f;
        amountXPRedColour.g = 0.0f;
        amountXPRedColour.b = 0.0f;
        amountXPRedColour.a = 1.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        amountXPText.text = Game.thePlayer.XP.ToString() + " XP";

        if (redXPTimer < redXPTime)
        {
            redXPTimer += Time.unscaledDeltaTime;
        }

        if (redXPTimer >= redXPTime)
        {
            amountXPText.color = amountXPNormalColour;
        }
    }

    public void FlashXPRed()
    {
        amountXPText.color = amountXPRedColour;
        redXPTimer = 0.0f;
    }

    public void ShowTooltip(string text, Vector3 topLeftPosition)
    {
        tooltipText.text = text;
        tooltipObject.transform.position = topLeftPosition;
        tooltipObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipText.text = "";
        tooltipObject.SetActive(false);
    }
}
