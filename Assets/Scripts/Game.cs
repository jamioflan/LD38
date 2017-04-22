using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start ()
    {
		
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

    private void ExecuteState_InMenus()
    {

    }

    private void ExecuteState_InLevel()
    {

    }

    private void ExecuteState_InInGameMenus()
    {
        // Check if we should return to game
        if (Input.GetAxis("Cancel") > 0)
        {
            SwitchToState(GameState.IN_LEVEL);
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
    }
}
