﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPiece : MonoBehaviour {

    public GameObject blocker, partialBlocker;
    public DungeonPiece piece;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Game.thePlayer.hasUpgrade("magicRangedFarsight"))
		{
			SetHidden(0);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() == Game.thePlayer)
        {
            piece.EnteredRoom();
            Game.thePlayer.numRoomsIn++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == Game.thePlayer)
        {
            piece.ExitedRoom();
            Game.thePlayer.numRoomsIn--;
        }
    }

    public void SetHidden(int hiddenState)
    {
        switch(hiddenState)
        {
            case 0: // Not hidden
                {
                    blocker.SetActive(false);
                    partialBlocker.SetActive(false);
                    break;
                }
            case 1: // Partially hidden
                {
                    blocker.SetActive(false);
                    partialBlocker.SetActive(true);
                    break;
                }
            case 2: // Hidden
                {
                    blocker.SetActive(true);
                    partialBlocker.SetActive(false);
                    break;
                }
        }
    }
}
