using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TimerUI : MonoBehaviour
{
    public float time;
    private int minutes, seconds;
    private TextMeshProUGUI textmeshPro;
    private void Awake()
    {
        textmeshPro = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        time -= Time.deltaTime;
        TimeConversion();
        TimeDisplay();
    }
    private void TimeConversion()
    {
        seconds = (int)time % 60;
        minutes = (int)time / 60;
    }
    private void TimeDisplay()
    {
        if ((minutes >= 10) && (seconds >= 10))
        {
            textmeshPro.SetText(minutes + ":" + seconds);
        }
        if ((minutes < 10) && (seconds >= 10))
        {
            textmeshPro.SetText("0" + minutes + ":" + seconds);
        }
        if ((minutes < 10) && (seconds < 10))
        {
            textmeshPro.SetText("0" + minutes + ":0" + seconds);
        }
        if ((minutes >= 10) && (seconds < 10))
        {
            textmeshPro.SetText("0" + minutes + ":0" + seconds);
        }   
    }
}
