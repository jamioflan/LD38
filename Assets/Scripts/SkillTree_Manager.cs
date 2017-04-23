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

	// Use this for initialization
	void Start ()
    {
        Game.theSkillTreeManager = this;

        amountXPText = amountXPObject.GetComponent<Text>();
        tooltipText = tooltipObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        amountXPText.text = Game.thePlayer.XP.ToString() + " XP";
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
