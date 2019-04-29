using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinCondition : MonoBehaviour
{
    public int moneyToWin = 10;
    public GameObject winText;
    public GameObject loseText;
    public static int winCondition;

    private void Start()
    {
        winCondition = moneyToWin;   
    }

    private void Update()
    {
        if (_LevelController.instance.player.money >= moneyToWin)
        {
            winText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {

                SceneManager.LoadScene(2);
            }
        }
        else if(_LevelController.instance.player.money <= 0)
        {
            loseText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {

                SceneManager.LoadScene(1);
            }
        }
    }
}
