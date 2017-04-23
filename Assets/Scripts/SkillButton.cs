using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
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
    }
	
	// Update is called once per frame
	void Update ()
    {

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
}
