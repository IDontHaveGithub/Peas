using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void MainLevel()
    {
        SceneManager.LoadScene("Testing");
    }

    public void SimonSays()
    {
        SceneManager.LoadScene("SimonSaysTest");
    }

    public void LightsOut()
    {
        SceneManager.LoadScene("LightOutTest");
    }

    public static void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
