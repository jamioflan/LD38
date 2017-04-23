using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public GameObject upgradeEntity;
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
