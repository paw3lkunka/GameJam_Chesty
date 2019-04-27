using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static _OurLib;

public class Player : _Creature
{
    public event Action LostEvent;

    public void AddMoney(uint x) => money += x;
    public void SubMoney(uint x)
    {
        if (money > x)
            money -= x;
        else
            LostEvent();
    }

    [SerializeField]
    private (bool state, int x, int y) coinsAutoCollect;

    public override void DropCoins(uint amount)
    {
        coinsAutoCollect = (false, X, Y);
        base.DropCoins( amount );
    }


    private void Awake()
    {
        LostEvent += _GameOver.GameOver;
    }

    private Vector2 currentFrameAxis;
    //private Vector2 previousFrameAxis;


    private new void Update()
    {
        /////TEMP////////
        if (Input.GetKeyUp(KeyCode.X))
        {
            DropCoins(5);
        }
        /////////////////


        base.Update();
        currentFrameAxis.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if( (currentFrameAxis.x != 0 || currentFrameAxis.y != 0))
        {
            if (currentFrameAxis.x > 0) MoveHoriz(1);
            else if (currentFrameAxis.x < 0) MoveHoriz(-1);
            else if (currentFrameAxis.y > 0) MoveVert(1);
            else if (currentFrameAxis.y < 0) MoveVert(-1);
        }


        if (coinsAutoCollect.state)
            CollectCoins();
        if (coinsAutoCollect.x != X || coinsAutoCollect.y != Y)
            coinsAutoCollect.state = true;

        //previousFrameAxis.Set(currentFrameAxis.x, currentFrameAxis.y);
    }

}
