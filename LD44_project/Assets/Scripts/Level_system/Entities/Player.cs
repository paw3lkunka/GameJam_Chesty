using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static _OurLib;

public class Player : _Creature
{

    public event Action LostEvent;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _LevelController.instance.MovementEvaluation += EvaluateMove;
        _LevelController.instance.MovementExecution += ExecuteMove;
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
    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }

    public override bool DealWithTrap(Trap trap)
    {
        money -= trap.Damage;
        trap.isArmed = false;
        return true;
    }

    public override bool DealWithKnight(Knight knight)
    {
        money -= knight.AttackPoints;
        return false;
    }

    public override bool DealWithMonster(Monster monster)
    {
        switch (_LevelController.instance.stage)
        {
            case 1:
                money -= monster.AttackPoints;
                return false;
            case 2:
                return true;
            default:
                throw new InvalidOperationException();
        }
    }

    public override bool DealWithDoor(Door door)
    {
        door.Open(this);
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
