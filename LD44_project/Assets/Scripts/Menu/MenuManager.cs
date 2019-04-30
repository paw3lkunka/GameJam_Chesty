using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_0");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LaunchLevel1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_0");//level1
    }
    public void LaunchLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_1");//level2
    }
    public void LaunchLevel3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_2");//level3
    }
    
}
