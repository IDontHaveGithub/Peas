using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonController : MonoBehaviour
{
    public GameObject Starter;

    public SimonController Instance;
    public ButtonB[] btns;

    public int simonMax;
    public float simonTime;

    private List<int> userList, simonList;
    public bool simonIsSaying;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        //set original values
        simonMax = 3;
        simonTime = 1f;

        simonList = new List<int>();//create list to keep adding

        StartCoroutine(SimonSays());
    }

    //keeps track of users actions
    public void PlayerAction(ButtonB b)
    {
        userList.Add(b.id);

        if (userList[userList.Count - 1] != simonList[userList.Count - 1])
        {
            Start();
            //Debug.Log("Lose");
        }
        if (userList.Count == simonList.Count)
        {
            // Debug.Log("Next level");
            if (simonMax < 8)
            {
                StartCoroutine(SimonSays());
            }
            else { Debug.Log("Open door"); }
        }
    }

    //write script to turn on the right camera and play the game
    public void GO()
    {
        //GameManager.others[/*find the right camera*/5].SetActive(true);
        StartCoroutine(SimonSays());
    }
    

    //creates the order of the saying
    IEnumerator SimonSays()
    {
       // Debug.Log("Prepare");
        yield return new WaitForSeconds(1);

        simonIsSaying = true;
        userList = new List<int>();

        if (simonMax <= 3)//create the first 3
        {
            for (int i = 0; i < simonMax; i++)
            {
                int rand = Random.Range(0, 4);
                simonList.Add(rand);
                btns[rand].Action();

                yield return new WaitForSeconds(simonTime);
            }
        }
        else //and add a new one
        {

            int rand = Random.Range(0, 4);
            simonList.Add(rand);

            for (int i = 0; i < simonMax; i++)
            {
                 btns[simonList[i]].Action();

                yield return new WaitForSeconds(simonTime);
            }
        }

        //prepare for next round and end this one
        simonTime -= 0.015f;
        simonMax++;
        simonIsSaying = false;
    }
}
