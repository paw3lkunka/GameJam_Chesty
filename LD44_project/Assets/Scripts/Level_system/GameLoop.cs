using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public static float globalSecondsLeft;
    [SerializeField]
    private float firstStageSeconds = 100f;
    private float secondStageSeconds = 100f;

    public GameObject stageOneText;
    public GameObject stageTwoText;
    public GameObject loseText;

    private void Start()
    {
        StartCoroutine(StartStageOne());
    }

    private void Update()
    {
        switch(_LevelController.instance.stage)
        {
            case 1:
                firstStageSeconds -= Time.deltaTime;
                globalSecondsLeft = firstStageSeconds;
                break;
            case 2:
                secondStageSeconds -= Time.deltaTime;
                globalSecondsLeft = firstStageSeconds;
                break;
        }
        if(globalSecondsLeft < 0)
        {
            switch (_LevelController.instance.stage)
            {
                case 1:
                    StartCoroutine(StartStageTwo());
                    _LevelController.instance.stage = 2;
                    break;
                case 2:
                    loseText.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    break;
            }
        }
    }

    public IEnumerator StartStageOne()
    {
        stageOneText.SetActive(true);
        yield return new WaitForSeconds(3f);
        stageOneText.SetActive(false);
    }

    private IEnumerator StartStageTwo()
    {
        stageTwoText.SetActive(true);
        yield return new WaitForSeconds(3f);
        stageTwoText.SetActive(false);
    }
}
