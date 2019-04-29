using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tour : MonoBehaviour
{
    public float tourTime;
    private Image tourBar;
    public static float fillAmount;
    
    private void Awake()
    {
        tourBar = GetComponent<Image>();
        fillAmount = 1;
    }

    void Update()
    {
        fillin();
        tourReset();
    }
    private void fillin()
    {
        fillAmount -= (Time.deltaTime/tourTime);
        tourBar.fillAmount = fillAmount;
    }
    private void tourReset()
    {
        if (fillAmount <=0)
        {
            fillAmount = 1;
        }
    }
}
