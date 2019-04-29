using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public static float globalSecondsLeft;
    [SerializeField]
    private float firstStageSeconds = 100f;
    private float secondStageSeconds = 100f;

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
    }
}
