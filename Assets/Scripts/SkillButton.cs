using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public GameObject upgradeEntity;
    public List<Image> linksFromDependencies = new List<Image>();

    Upgrade upgrade;

	// Use this for initialization
	void Start ()
    {
        upgrade = upgradeEntity.GetComponent<Upgrade>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnClick()
    {
        SkillTree_Manager.OnClick(upgrade);
    }
}
