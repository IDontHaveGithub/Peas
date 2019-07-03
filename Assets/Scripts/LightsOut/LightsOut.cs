using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOut : MonoBehaviour
{
    public class Cell
    {
        public bool visited;
        public Light light;
    }

    public GameObject cell;

    private float wallLength;
    public static int xSize = 5;
    public static int ySize = 5;
    private Vector3 initialPos;

    private GameObject wallHolder;
    
    public Cell[] cells;
    public int currentCell = 0;
    private int totalCells;

    public List<GameObject> lights;
    public List<int> neighbours = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        wallLength = cell.transform.localScale.z;
       // main.orthographicSize = xSize;
        CreateCells();
    }

    void CreateCells()
    {
        totalCells = xSize * ySize;
        wallHolder = new GameObject();
        wallHolder.name = "LightsOut";
        wallHolder.transform.parent = transform;
        wallHolder.transform.position = transform.position;

        initialPos = new Vector3(wallHolder.transform.position.x-.5f, wallHolder.transform.position.y, wallHolder.transform.position.z-.5f);
        Vector3 myPos = initialPos;
        GameObject tempCell;

        cells = new Cell[xSize * ySize];

        //for x axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, .4f, initialPos.z + (i * wallLength) - wallLength / 2);
                tempCell = Instantiate(cell, myPos, Quaternion.identity, wallHolder.transform) as GameObject;
            }
        }
        

        GetLights();
    }

    void GetLights()
    {
        int children = wallHolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];

        //get all children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
            cells[i] = new Cell();
            cells[i].light = allWalls[i].GetComponentInChildren<Light>();
            allWalls[i].name = "" + i;
        }

        wallHolder.transform.localScale = wallHolder.transform.localScale / 3;
    }

    private void GiveNeighbour()
    {
        int length = 0;
        neighbours.Clear();
        int check = ((((currentCell + 1) / xSize) - 1) * xSize) + xSize;

        //east
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                neighbours.Add(currentCell + 1);
                length++;
            }
        }


        //west
        if (currentCell + 1 > 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbours.Add(currentCell - 1);
                length++;
            }
        }

        //north
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbours.Add(currentCell + xSize);
                length++;
            }
        }

        //south
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbours.Add(currentCell - xSize);
                length++;
            }
        }
    }

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
            Debug.Log("Win");
        }

    }
}
