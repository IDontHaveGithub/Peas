using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOut : MonoBehaviour
{
    //uses a lot of the maze script, but changed to accommodate only cells.

    public class Cell
    {
        public bool visited;
        public Light light;
    }

    public int currentCell = 0;

    private float wallLength;

    private int XSize = 5;
    private int YSize = 5;
    private int TotalCells;

    private Vector3 initialPos;

    public GameObject cell;
    private GameObject cellHolder;
    
    public Cell[] cells;
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
        TotalCells = XSize * YSize;

        //create parent object to keep hierarchy readable
        cellHolder = new GameObject();
        cellHolder.name = "LightsOut";
        cellHolder.transform.parent = transform;
        cellHolder.transform.position = transform.position;

        //initial position is position of parent object with offset and not O-point of world as previous
        initialPos = new Vector3(cellHolder.transform.position.x-.5f, cellHolder.transform.position.y, cellHolder.transform.position.z-.5f);
        Vector3 myPos = initialPos;
        GameObject tempCell;

        cells = new Cell[XSize * YSize];

        //because we just need one way of objects now, since it would become way more, just x-axis spawns
        for (int i = 0; i < YSize; i++)
        {
            for (int j = 0; j < XSize; j++)
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

        cellHolder.transform.localScale /= 3f;
    }


    // get all neighbours
    private void GiveNeighbour()
    {
        int length = 0;
        neighbours.Clear();
        int check = ((((currentCell + 1) / XSize) - 1) * XSize) + XSize;

        //east neighbour
        if (currentCell + 1 < TotalCells && (currentCell + 1) != check)
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
        if (currentCell + XSize < TotalCells)
        {
            if (cells[currentCell + XSize].visited == false)
            {
                neighbours.Add(currentCell + XSize);
                length++;
            }
        }

        //south neighbour
        if (currentCell - XSize >= 0)
        {
            if (cells[currentCell - XSize].visited == false)
            {
                neighbours.Add(currentCell - XSize);
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

        for (int i = 0; i < cells.Length; i++)
        {
            if(cells[i].light.enabled)
            {
                cellCount++;
            }
        }
        Debug.Log(cellCount);
        if (cellCount == 0)
        {
            //TODO: write WinCondition
            Debug.Log("Win"); //that's not wincondition
            LevelManager.MainMenu();
        }
    }
}
