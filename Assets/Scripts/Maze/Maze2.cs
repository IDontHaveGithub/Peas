using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze2 : MonoBehaviour
{
    //own class for every cell to assign walls easier and give them their own bool
    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north, west, south, east;
    }

    //camera variable to automatically place camera in right place
    //public Camera main;

    public Vector3 myPos;
    public GameObject[] cell = new GameObject[5];

    //spawning objects and size of said object
    public GameObject wall;
    public GameObject door;
    private float wallLength;

    //size of maze
    public int xSize = 5;
    public int ySize = 5;

    //placement of first wall
    private Vector3 initialPos = new Vector3(0, 0.5f, 0);

    //objects to hold the others inside
    private GameObject wallHolder;
    private GameObject cellHolder;

    [SerializeField]
    private Cell[] cells;
    private int currentCell = 0;
    private int totalCells;

    private Vector3[] cellPosition;
    private int vistitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbour = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;

    //simple colours to assign to random cells, to check if I can spawn in different rooms
    //private Renderer cellColour;
    //private List<Color> colors = new List<Color>()
    //{
    //    Color.red,Color.blue,Color.green,Color.yellow,Color.magenta
    //};


    // Start is called before the first frame update
    void Start()
    {
        //check size wallobject
        wallLength = wall.transform.localScale.z*2;


       // main.transform.position = new Vector3(xSize * wallLength, 20, ySize * wallLength);
        //main.orthographicSize = xSize * 4;


        CreateWalls();
    }

    //start maze creation
    void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        //initialPos = new Vector3((-xSize / 2) + wallLength / 2, 0.0f, (-ySize / 2) + wallLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempWall;


        //for vertical walls
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength * 3) - wallLength / 2, 1.5f, initialPos.z + (i * wallLength * 3) + wallLength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 90f, 0f), wallHolder.transform) as GameObject;
            }
        }

        //for horizontal walls
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength * 3) + wallLength, 1.5f, initialPos.z + (i * wallLength * 3) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 0f, 0f), wallHolder.transform) as GameObject;
            }
        }


        CreateCells();
    }

    //assign walls to cells, to actually create a cell-like structure and not just walls
    void CreateCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;

        GameObject[] allWalls;
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[xSize * ySize];
        int westeastProcess = 0;
        int childProcess = 0;
        int termcount = 0;

        //get all children
        for (int i = 0; i < children; i++)
        {            
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }
        //assigns walls to cells
        for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
        {
            if (termcount == xSize)
            {
                westeastProcess++;
                termcount = 0;
            }

            cells[cellProcess] = new Cell();
            cells[cellProcess].west = allWalls[westeastProcess];
            cells[cellProcess].south = allWalls[childProcess + (xSize + 1) * ySize];

            westeastProcess++;
            termcount++;
            childProcess++;

            cells[cellProcess].east = allWalls[westeastProcess];
            cells[cellProcess].north = allWalls[(childProcess + (xSize + 1) * ySize) + xSize - 1];
        }
        GetCellPosition();
        CreateMaze();
    }

    //get middle of cell position to prepare for spawning rooms
    void GetCellPosition()
    {
        cellPosition = new Vector3[cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {
            cellPosition[i] = cells[i].west.transform.position + new Vector3(wallLength + wallLength / 2, -1.4f, 0);
        }
        MakeCells();

       // main.transform.position = cellPosition[Mathf.RoundToInt(cells.Length / 2)];
       // main.transform.position = new Vector3(main.transform.position.x, 20, main.transform.position.z);
    }

    //spawn "rooms" for now colored cubes
    void MakeCells()
    {
        cellHolder = new GameObject();
        cellHolder.name = "cells";
        cellHolder.transform.SetParent(wallHolder.transform);

        GameObject tempCell;

        int end = Random.Range(1, 25);
        for (int i = 0; i < cellPosition.Length; i++)
        {
            //spawn random "room"
            //now still a coloured cube, also still spawns "room" first than changes colour, needs to spawn random coloured cube.

            
            if (i == 0)
            {
                tempCell = Instantiate(cell[4], cellPosition[i], Quaternion.identity, cellHolder.transform);
            }
            else if (i == end)
            {
                tempCell = Instantiate(cell[3], cellPosition[i], Quaternion.identity, cellHolder.transform);
            }
            else
                tempCell = Instantiate(cell[Random.Range(0,3)], cellPosition[i], Quaternion.identity, cellHolder.transform);
            //cellColour = tempCell.GetComponent<Renderer>();
            //cellColour.material.color = colors[Random.Range(0,5)];


            //create more walls that are permanent to make maze seem bigger and create doorways instead of missing walls
            GameObject permWall;

            //for vertical walls
            for (int x = 0; x < ySize; x++)
            {
                for (int j = 0; j <= xSize; j++)
                {
                    myPos = new Vector3(initialPos.x + (j * wallLength * 3) - wallLength / 2, 1.5f, initialPos.z + (x * wallLength * 3) - wallLength / 2);
                    permWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 90f, 0f), wallHolder.transform) as GameObject;

                    myPos = new Vector3(initialPos.x + (j * wallLength * 3) - wallLength / 2, 1.5f, initialPos.z + (x * wallLength * 3) + wallLength * 1.5f);
                    permWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 90f, 0f), wallHolder.transform) as GameObject;
                }
            }

            //for horizontal walls
            for (int x = 0; x <= ySize; x++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    myPos = new Vector3(initialPos.x + (j * wallLength * 3), 1.5f, initialPos.z + (x * wallLength * 3) - wallLength);
                    permWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 0f, 0f), wallHolder.transform) as GameObject;

                    myPos = new Vector3(initialPos.x + (j * wallLength * 3) + wallLength * 2, 1.5f, initialPos.z + (x * wallLength * 3) - wallLength);
                    permWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 0f, 0f), wallHolder.transform) as GameObject;
                }
            }
        }
    }

    //delete random walls from cells to create the maze.
    void CreateMaze()
    {
        while (vistitedCells < totalCells)
        {
            if (startedBuilding)
            {
                GiveMeNeighbour();
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true)
                {
                    BreakWall();
                    cells[currentNeighbour].visited = true;
                    vistitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                vistitedCells++;
                startedBuilding = true;
            }
        }
    }

    //the actual breaking of the walls
    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1:
              //  Instantiate(door, cells[currentCell].north.transform.position, Quaternion.Euler(-90f, 0f, 0f), cellHolder.transform);
                Destroy(cells[currentCell].north); break;
            case 2:
             //   Instantiate(door, cells[currentCell].west.transform.position, Quaternion.Euler(-90f, 90f, 0f), cellHolder.transform);
                Destroy(cells[currentCell].west); break;
            case 3:
              //  Instantiate(door, cells[currentCell].south.transform.position, Quaternion.Euler(-90f, 0f, 0f), cellHolder.transform);
                Destroy(cells[currentCell].south); break;
            case 4:
             //   Instantiate(door, cells[currentCell].east.transform.position, Quaternion.Euler(-90f, 90f, 0f), cellHolder.transform);
                Destroy(cells[currentCell].east); break;
        }
    }

    //get the neighbours to decide the way of the maze randomly and create maze.
    void GiveMeNeighbour()
    {
        int length = 0;
        int[] neighbours = new int[4];
        int[] connectingWall = new int[4];
        int check = ((((currentCell + 1) / xSize)-1)*xSize)+xSize;

        //east
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                neighbours[length] = currentCell + 1;
                connectingWall[length] = 4;
                length++;
            }
        }

        //west
        if (currentCell + 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbours[length] = currentCell - 1;
                connectingWall[length] = 2;
                length++;
            }
        }

        //north
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbours[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }

        //south
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbours[length] = currentCell - xSize;
                connectingWall[length] = 3;
                length++;
            }
        }

        if (length != 0)
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbour = neighbours[theChosenOne];
            wallToBreak = connectingWall[theChosenOne];
        }
        else
        {
            if(backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }
}
