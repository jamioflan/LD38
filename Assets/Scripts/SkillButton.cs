using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject upgradeEntity;
    public List<Image> linksFromDependencies = new List<Image>();
    public GameObject skillOwnedImage;

    Upgrade upgrade;

	// Use this for initialization
	void Start ()
    {
        upgrade = upgradeEntity.GetComponent<Upgrade>();

        foreach (Image link in linksFromDependencies)
        {
            link.gameObject.SetActive(false);
        }

        skillOwnedImage.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if( upgrade.isOwnedOrAvailableToUnlock() )
        {
            Color buttonColour = GetComponent<Image>().color;
            buttonColour.a = 1.0f;
            GetComponent<Image>().color = buttonColour;
        }
        else
        {
            Color buttonColour = GetComponent<Image>().color;
            buttonColour.a = 0.2f;
            GetComponent<Image>().color = buttonColour;
        }
	}

    public void OnClick()
    {
        // Check if it's available and we can afford it
        if (upgrade.isAvailableToUnlock() && upgrade.canAfford())
        {
			upgrade.grant(Game.thePlayer);

            foreach (Image link in linksFromDependencies)
            {
                link.gameObject.SetActive(true);
            }

            skillOwnedImage.SetActive(true);
        }
    }

    public void OnPointerEnter( PointerEventData data )
    {
        string tooltip = upgrade.cost.ToString() + " XP\n" + upgrade.description;
        Game.theSkillTreeManager.ShowTooltip(tooltip);
    }

    public void OnPointerExit( PointerEventData data )
    {
        Game.theSkillTreeManager.HideTooltip();
    }
}
