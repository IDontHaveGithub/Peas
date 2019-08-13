﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Camera main;

    public static GameObject[] others;
    
    public static bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //little delay to spawn everything correctly first
        Invoke("GetCams", 0.001f);
        
    }

    void GetCams()
    {
        others = GameObject.FindGameObjectsWithTag("Camera");
        if (main.enabled)
        {
            for (int i = 0; i < others.Length; i++)
            {
                others[i].SetActive(false);
            }
        }
    }

    //start any puzzle
    public static void StartGame()
    {
        Debug.Log("This is where I need to have the right cell to turn on the right camera and start the right game.");
    }

    //quit the game after finishing, cause no menu
    public static void EndGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
