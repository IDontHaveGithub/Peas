using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north, west, south, east;
    }

    //public Camera main;

    public Vector3 myPos;

    public GameObject wall;
    private float wallLength;
    public int xSize = 5;
    public int ySize = 5;
    private Vector3 initialPos = new Vector3(0, 0.5f, 20);
    private GameObject wallHolder;

    [SerializeField]
    private Cell[] cells;
    private int currentCell = 0;
    private int totalCells;

    private int vistitedCells = 0;
    private bool startedBuilding = false;

    private int currentNeighbour = 0;

    private List<int> lastCells;
    private int backingUp = 0;

    private int wallToBreak = 0;


    // Start is called before the first frame update
    void Start()
    {
        wallLength = wall.transform.localScale.z;
       // main.transform.position = new Vector3(xSize / 2, 100, ySize / 2);
       // main.orthographicSize = xSize+2;
        CreateWalls();
    }

    void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        //initialPos = new Vector3((-xSize / 2) + wallLength / 2, 0.0f, (-ySize / 2) + wallLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempWall;


        //for x axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, 1.5f, initialPos.z + (i * wallLength) - wallLength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 90f, 0f), wallHolder.transform) as GameObject;
            }
        }

        //for Y axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength), 1.5f, initialPos.z + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(-90f, 0f, 0f), wallHolder.transform) as GameObject;
            }
        }

        CreateCells();
    }

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
        CreateMaze();
    }

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

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1:Destroy(cells[currentCell].north); break;
            case 2:Destroy(cells[currentCell].west); break;
            case 3:Destroy(cells[currentCell].south); break;
            case 4:Destroy(cells[currentCell].east); break;
        }
    }

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
