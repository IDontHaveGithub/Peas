using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonController : MonoBehaviour
{
    public float simonTime;
    public int simonMax;
    public bool simonIsSaying;

    public ButtonB[] btns;

    public SimonController Instance;
    private List<int> userList, simonList;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        //set original values
        simonMax = 3;
        simonTime = 1f;

        simonList = new List<int>();//create list to keep adding

        StartCoroutine(SimonSays());//TODO: now it starts at initialisation, it only has to start when called upon
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
            else
            {
                Debug.Log("Open door");
                LevelManager.MainMenu();
            }
        }
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
