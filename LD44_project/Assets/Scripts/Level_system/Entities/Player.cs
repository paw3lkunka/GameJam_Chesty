using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static _OurLib;

public class Player : _Creature
{
    [SerializeField]
    private (bool state, int x, int y) coinsAutoCollect;

    public static bool repeatMovement = true;

    public event Action LostEvent;
    public event Action OnTileEnterPlayer;

    private void Awake()
    {
        _LevelController.instance.ForceMovement += Move;
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

    public override void DropCoins(uint amount)
    {
        coinsAutoCollect = (false, X, Y);
        base.DropCoins( amount );
    }

    private new void Update()
    {
        base.Update();
        
        if (coinsAutoCollect.state)
            CollectCoins();
        if (coinsAutoCollect.x != X || coinsAutoCollect.y != Y)
            coinsAutoCollect.state = true;
    }

    protected override void EndMovement()
    {
        base.EndMovement();
        Player.repeatMovement = true;
    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }
    public override bool DealWithTrap()
    {
        return !((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).thing as Trap).armed;
    }

    public override bool DealWithKnight()
    {
        //SubMoney(((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).agent as Knight).AttackPoints);
        SubMoney((CurrentTilePlus<Floor>(movementVector.x, movementVector.y).agent as Knight).AttackPoints);
        return false;
    }

    public override bool DealWithMonster()
    {
        switch (_LevelController.instance.stage)
        {
            case 1:
                return false;
            case 2:
                return true;
            default:
                throw new InvalidOperationException();
        }
    }

    public override bool DealWithDoor()
    {
        (_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Door).Open(this);
        return true;
    }

}
