using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static _OurLib;

public class Player : _Creature
{
    [SerializeField]
    private (bool state, int x, int y) coinsAutoCollect;

    public event Action LostEvent;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _LevelController.instance.ForceMovement += Move;
        LostEvent += _GameOver.GameOver;
    }

    public override void DropCoins(int amount)
    {
        coinsAutoCollect = (false, X, Y);
        base.DropCoins( amount );
    }

    protected override void Update()
    {
        base.Update();
        
        if (coinsAutoCollect.state)
            CollectCoins();
        if (coinsAutoCollect.x != X || coinsAutoCollect.y != Y)
            coinsAutoCollect.state = true;
    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }

    public override bool DealWithTrap()
    {
        return !((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).thing as Trap).isArmed;
    }

    public override bool DealWithKnight()
    {
        //SubMoney(((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).agent as Knight).AttackPoints);
        //money -= CurrentTilePlus<Floor>(movementVector.x, movementVector.y).agent as Knight).AttackPoints;
        transform.position = new Vector2(X - movementVector.x, Y - movementVector.y);
        return false;
    }

    public override bool DealWithMonster()
    {
        switch (_LevelController.instance.stage)
        {
            case 1:
                // THIS IS TEMPORARY AND SHOULD BE CHANGED
                money -= 1;
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

    public override bool DealWithPlayer()
    {
        throw new NotImplementedException();
    }

    public override void Die()
    {
        throw new NotImplementedException();
    }
}
