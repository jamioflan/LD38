using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        IN_MENUS,
        IN_LEVEL,
        IN_INGAME_MENUS,
        SHIFTING,
    }

    public GameState state = GameState.IN_MENUS;
    public float timeInState = 0.0f;
    public float shiftTime = 3.0f;

    public static Player thePlayer = null;

    public GameObject menuHUD;
    public GameObject menuStart;
    public GameObject menuSkills;

	// Use this for initialization
	void Start ()
    {
//        menuHUD = transform.Find("HUD").gameObject;
//        menuStart = transform.Find("Start").gameObject;
//        menuSkills = transform.Find("SkillTree_Menu").gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeInState += Time.deltaTime;
        switch (state)
        {
            case GameState.IN_MENUS:              { ExecuteState_InMenus();          break; }
            case GameState.IN_LEVEL:              { ExecuteState_InLevel();          break; }
            case GameState.IN_INGAME_MENUS:       { ExecuteState_InInGameMenus();    break; }
            case GameState.SHIFTING:              { ExecuteState_Shifting();         break; }
        }
	}

    public void OnPressStart()
    {
        menuStart.SetActive(false);
        StartNextLevel();
    }

    private void ExecuteState_InMenus()
    {

    }

    private void ExecuteState_InLevel()
    {
        // Check if we should open the skill tree menu
        if (Input.GetAxis("Tab") > 0)
        {
            SwitchToState(GameState.IN_INGAME_MENUS);
            menuHUD.SetActive(false);
        }
        else
        {
            menuHUD.SetActive(true);
        }
    }

    private void ExecuteState_InInGameMenus()
    {
        // Check if we should return to game
        if (Input.GetAxis("Cancel") > 0)
        {
            SwitchToState(GameState.IN_LEVEL);
            menuSkills.SetActive(false);
        }
        else
        {
            menuSkills.SetActive(true);
        }
    }

    private void ExecuteState_Shifting()
    {
        if (timeInState >= shiftTime)
        {
            StartNextLevel();
        }
    }

    private void StartNextLevel()
    {
        SwitchToState(GameState.IN_LEVEL);
        // Set new start and end nodes

        // Tell generator to work on the next setup
    }

    private void SwitchToState(GameState newState)
    {
        state = newState;
        timeInState = 0.0f;
    }
}
