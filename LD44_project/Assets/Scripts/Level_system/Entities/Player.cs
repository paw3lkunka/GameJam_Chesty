using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static _OurLib;

public class Player : _Creature
{
    public event Action LostEvent;

    private Vector2 currentFrameAxis;
    private Vector2 previousFrameAxis;

    private bool isMovingLastFrame;

    private void Awake()
    {
        LostEvent += _GameOver.GameOver;
    }

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

        if( currentFrameAxis.x != previousFrameAxis.x || currentFrameAxis.y != previousFrameAxis.y || repeatMovement == true)
        {
            if (currentFrameAxis.x > 0) MoveHoriz(1);
            else if (currentFrameAxis.x < 0) MoveHoriz(-1);
            else if (currentFrameAxis.y > 0) MoveVert(1);
            else if (currentFrameAxis.y < 0) MoveVert(-1);
        }

        previousFrameAxis.Set(currentFrameAxis.x, currentFrameAxis.y);

        if (coinsAutoCollect.state)
            CollectCoins();
        if (coinsAutoCollect.x != X || coinsAutoCollect.y != Y)
            coinsAutoCollect.state = true;

    }

}
