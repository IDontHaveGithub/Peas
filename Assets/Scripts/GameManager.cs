using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Camera main;

    public static GameObject[] others;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        Invoke("GetCams", 1);
        
    }

    // Update is called once per frame
    void Update()
    {
       
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

    public static void StartGame()
    {
        Debug.Log("This is where I need to have the right cell to turn on the right camera and start the right game.");
    }

    public static void EndGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
