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
        IN_ENDGAME_MENUS,
        SHIFTING,
    }

    public GameState state = GameState.IN_MENUS;
    public float timeInState = 0.0f;
    public float unscaledTimeInState = 0.0f;
    public float shiftTime = 3.0f;

    public static Player thePlayer = null;

    public GameObject menuHUD;
    public GameObject menuStart;
    public GameObject menuSkills;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        timeInState += Time.deltaTime;
        unscaledTimeInState += Time.unscaledDeltaTime;
        switch (state)
        {
            case GameState.IN_MENUS:              { ExecuteState_InMenus();          break; }
            case GameState.IN_LEVEL:              { ExecuteState_InLevel();          break; }
            case GameState.IN_INGAME_MENUS:       { ExecuteState_InInGameMenus();    break; }
            case GameState.IN_ENDGAME_MENUS:      { ExecuteState_InEndGameMenus();   break; }
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
        // Is the player dead?
        if (thePlayer.health <= 0.0f)
        {
            SwitchToState(GameState.IN_ENDGAME_MENUS);
            return;
        }

        // Check if we should open the skill tree menu
        // Be careful that a Tab press to close the skills menu doesn't immediately
        // open it again
        if (unscaledTimeInState > 0.2 && Input.GetAxisRaw("Tab") > 0)
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
        // Allow Esc or Tab to close the menu, but be careful that the Tab press that opens
        // the menu doesn't close it again instantly.
        if (Input.GetAxisRaw("Cancel") > 0 || (unscaledTimeInState > 0.2 && Input.GetAxisRaw("Tab") > 0))
        {
            SwitchToState(GameState.IN_LEVEL);
            menuSkills.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            menuSkills.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    private void ExecuteState_InEndGameMenus()
    {

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
        unscaledTimeInState = 0.0f;
    }
}
