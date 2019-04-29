using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game manager main class. Should not be attached manually as an component as it is instatniated automatically by Loader script.
/// </summary>
public class _GameManager : MonoBehaviour
{
    public static _GameManager instance = null;

    private void Awake()
    {
        //Time.timeScale = 1;

        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        // Prevent the game manager from being destroyed while loading scenes
       // DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
