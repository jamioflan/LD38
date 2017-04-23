using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree_Manager : MonoBehaviour
{
    public GameObject amountXPObject;
    public GameObject tooltipObject;
    public Vector3 tooltipOffset;

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
        amountXPText.text = Game.thePlayer.XP.ToString();
    }

    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipObject.transform.position = Input.mousePosition + tooltipOffset;
        tooltipObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipText.text = "";
        tooltipObject.SetActive(false);
    }
}
