using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        IN_MENUS,
        IN_LEVEL,
        SHIFTING,
    }

    public GameState state = GameState.IN_MENUS;
    public float timeInState = 0.0f;
    public float shiftTime = 3.0f;

    //public Player thePlayer = null;

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
            case GameState.IN_MENUS:
            {
                break;
            }
            case GameState.IN_LEVEL:
            {
                break;
            }
            case GameState.SHIFTING:
            {
                if(timeInState >= shiftTime)
                {
                    StartNextLevel();
                }
                break;
            }
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
