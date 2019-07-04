using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOut : MonoBehaviour
{
    //uses a lot of the maze script, but changed to accommodate only cells.

    //get the cell and it's light
    public class Cell
    {
        public bool visited;
        public Light light;
    }

    public GameObject cell;

    //get the basics of the grid
    private float wallLength; // "cell"length
    public static int xSize = 5;
    public static int ySize = 5;
    private Vector3 initialPos;

    private GameObject cellHolder;
    
    //anything to do with cells
    public Cell[] cells;
    public int currentCell = 0;
    private int totalCells;

    //store all lights and neigbours
    public List<GameObject> lights;
    public List<int> neighbours = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        wallLength = cell.transform.localScale.z;
        CreateCells();
    }

    //create cells
    void CreateCells()
    {
        totalCells = xSize * ySize;
        cellHolder = new GameObject();
        cellHolder.name = "LightsOut";
        cellHolder.transform.parent = transform;
        cellHolder.transform.position = transform.position;

        //initial position is position of parent object with offset and not O-point of world as previous
        initialPos = new Vector3(cellHolder.transform.position.x-.5f, cellHolder.transform.position.y, cellHolder.transform.position.z-.5f);
        Vector3 myPos = initialPos;
        GameObject tempCell;

        cells = new Cell[xSize * ySize];

        //becaue we just need one way of objects now, since it would become way more, just x-axis spawns
        //for x axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, .4f, initialPos.z + (i * wallLength) - wallLength / 2);
                tempCell = Instantiate(cell, myPos, Quaternion.identity, cellHolder.transform) as GameObject;
            }
        }

        GetLights();
    }

    void GetLights()
    {
        int children = cellHolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];

        //get all children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = cellHolder.transform.GetChild(i).gameObject;
            cells[i] = new Cell();
            cells[i].light = allWalls[i].GetComponentInChildren<Light>();
            allWalls[i].name = "" + i;
        }

        cellHolder.transform.localScale = cellHolder.transform.localScale / 3;
    }


    // get all neighbours
    private void GiveNeighbour()
    {
        int length = 0;
        neighbours.Clear();
        int check = ((((currentCell + 1) / xSize) - 1) * xSize) + xSize;

        //east neighbour
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                neighbours.Add(currentCell + 1);
                length++;
            }
        }


        //west neighbour
        if (currentCell + 1 > 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbours.Add(currentCell - 1);
                length++;
            }
        }

        //north neighbour
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbours.Add(currentCell + xSize);
                length++;
            }
        }

        //south neighbour
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbours.Add(currentCell - xSize);
                length++;
            }
        }
    }

    //turn off lights from current cell and neighbouring cells.
    public void LightsOff()
    {
        GiveNeighbour();

        cells[currentCell].light.enabled = !cells[currentCell].light.enabled;
        for (int i = 0; i < neighbours.Count; i++)
        {
            // get cell light to turn off
            cells[neighbours[i]].light.enabled = !cells[neighbours[i]].light.enabled;
        }

        WinCheck();
    }

    //check if all lights off
    private void WinCheck()
    {
        int cellCount = 0;

        for (int i = 0; i <cells.Length; i++)
        {
            if(cells[i].light.enabled)
            {
                cellCount++;
               
            }
        }
        Debug.Log(cellCount);
        if (cellCount == 0)
        {
            //write WinCondition
            Debug.Log("Win"); //that's not wincondition
        }

    }
}
