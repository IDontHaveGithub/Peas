﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    //because it's not done yet, for now a button to open the next door, later done by finishing a puzzle of following the parkour.

    public Button openDoor;
    public Text currentRoom;

    public Maze2 maze2;

    // Start is called before the first frame update
    void Start()
    {
        openDoor.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.start)
        {
            openDoor.enabled = true;
        }
        else { openDoor.enabled = false; }

        if(openDoor.enabled && Input.GetKeyDown(KeyCode.E))
        {
            OpenTheDoor(0);
        }
    }

    public void OpenTheDoor(int currentRoom)
    {
        maze2.CreateMaze();
    }
}