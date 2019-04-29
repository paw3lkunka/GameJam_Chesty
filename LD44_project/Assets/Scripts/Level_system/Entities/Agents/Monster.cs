using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;
using System;

public class Monster : _Agent
{

    private new void Start()
    {
        base.Start();

        //_LevelController.instance.MovementEvaluation += MonsterAI;
        _LevelController.instance.MovementEvaluation += StepForwards;
        _LevelController.instance.MovementExecution += ExecuteMove;
    }

    private new void Update()
    {

    }

    public override void Die()
    {
        _LevelController.instance.MovementEvaluation -= StepForwards;
        _LevelController.instance.monsters.Remove(this);
        Destroy(gameObject);
    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }

    public override bool DealWithTrap(Trap trap)
    {
        return true;
    }

    public override bool DealWithKnight(Knight knight)
    {
        Fight((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).agent as Knight);
        return true;
    }

    public override bool DealWithMonster(Monster monster)
    {
        return true;
    }

    public override bool DealWithDoor(Door door)
    {
        return false;
    }

    public override bool DealWithPlayer()
    {
        return false;
    }
}
