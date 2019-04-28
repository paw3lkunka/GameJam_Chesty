using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinCondition : MonoBehaviour
{
    public int moneyToWin = 10;

    private void Update()
    {
        if (_LevelController.instance.player.money >= moneyToWin)
        {
            SceneManager.LoadScene(1);
        }
    }
}
