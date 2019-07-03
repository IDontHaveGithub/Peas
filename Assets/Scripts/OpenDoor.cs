using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    public Button openDoorsie;
    public Text currentRoomsie;

    public Maze2 maze2;

    // Start is called before the first frame update
    void Start()
    {
        openDoorsie.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.start)
        {
            openDoorsie.enabled = true;
        }
        else { openDoorsie.enabled = false; }
    }

    public void OpenZeDoor(int currentRoom)
    {
        maze2.CreateMaze();
    }
}
