﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

	// Use this for initialization
	void Start ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        // Hacky way to trigger it
        if(Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }

    public void Generate()
    {

    }
}
