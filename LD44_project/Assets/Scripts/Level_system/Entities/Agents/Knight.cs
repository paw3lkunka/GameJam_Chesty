using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;

public class Knight : _Agent
{
#pragma warning disable
    [SerializeField]
    private float sightRange = 10f;
#pragma warning restore
    [SerializeField] private int greed = 1;
    [SerializeField] private int honour = 1;
    [SerializeField] private int fear = 1;

    private float distanceToPlayer = 0f;
    private (float, Monster) distanceToMonster;
    private float distanceToCoin = 0f;

    private List<(float, Monster)> monsterDistances;

    private enum KnightStates { DoingNothing, LookingAround, ChasingPlayer, FleeingMonster, GettingCoin }
    [SerializeField]
    private KnightStates knightState = KnightStates.DoingNothing;
    private KnightStates lastKnightState = KnightStates.DoingNothing;

    private new void Start()
    {
        base.Start();
        monsterDistances = new List<(float, Monster)>();
        // create a grid
        
        _LevelController.instance.ForceMovement += KnightAI;
        _LevelController.instance.ForceMovement += StepForwards;
    }

    private new void Update()
    {
        Debug.Log("Dist. to player: " + distanceToPlayer + " Dist. to monster: " + distanceToMonster.Item1);
        base.Update();
    }

    private void KnightAI(int a, int b)
    {
        Decision();
        if(!lastKnightState.Equals(knightState))
        {
            _from.Set(X, Y);
            switch (knightState)
            {
                case KnightStates.DoingNothing:
                    _to.Set(X, Y);
                    break;
                case KnightStates.LookingAround:
                    RandomTileTarget();
                    break;
                case KnightStates.ChasingPlayer:
                    _to.Set(_LevelController.instance.player.X, _LevelController.instance.player.Y);
                    break;
                case KnightStates.FleeingMonster:
                    _to.Set(_LevelController.instance.player.X, _LevelController.instance.player.Y);
                    break;
                case KnightStates.GettingCoin:
                    _to.Set(X, Y);
                    break;
            }

            path = PathFind.Pathfinding.FindPath(grid, _from, _to);
            ExtrapolatePath(path);
        }

        int e = (int)knightState;
        lastKnightState = (KnightStates)e;
        //if (_from == _to && knightState == KnightStates.LookingAround) RandomTileTarget(); 

    }

    private void Decision()
    {
        // Check if can see player
        RaycastHit2D linecastHit = Physics2D.Linecast(transform.position, _LevelController.instance.player.transform.position);
        if (linecastHit.transform.tag == "Player")
            distanceToPlayer = linecastHit.distance;
        else
            distanceToPlayer = 0f;

        // Check if can see Monster
        monsterDistances.Clear();
        foreach (Monster element in _LevelController.instance.monsters)
        {
            monsterDistances.Add((CalculateDistanceSqr(transform.position, element.transform.position), element));
        }

        distanceToMonster.Item1 = 0f;
        distanceToMonster.Item2 = null;
        monsterDistances.Sort(CompareMonsters);
        for (int i = 0; i < monsterDistances.Count; i++)
        {
            RaycastHit2D linecastMonster = Physics2D.Linecast(transform.position, monsterDistances[i].Item2.transform.position);
            if (linecastMonster.transform.tag == "Monster")
            {
                distanceToMonster = monsterDistances[i];
                break;
            }
        }
        
        // Check for coin
        // to implement

        if(distanceToPlayer == 0 && distanceToMonster.Item1 == 0)
        {
            knightState = KnightStates.LookingAround;
        }
        else if(distanceToPlayer == 0 && distanceToMonster.Item1 > 0)
        {
            knightState = KnightStates.FleeingMonster;
        }
        else if(distanceToMonster.Item1 == 0 && distanceToPlayer > 0)
        {
            knightState = KnightStates.ChasingPlayer;
        }
        if(distanceToPlayer * honour < distanceToMonster.Item1 / fear)
        {
            knightState = KnightStates.ChasingPlayer;
        }
        else
        {
            knightState = KnightStates.LookingAround;
        }
    }

    private static int CompareMonsters( (float, Monster) monsterA, (float, Monster) monsterB )
    {
        return (int)(monsterB.Item1 - monsterA.Item1);
    }

    private float CalculateDistanceSqr(Vector2 from, Vector2 to)
    {
        return (to.x - from.x) * (to.x - from.x) + (to.y - from.y) * (to.y - from.y);
    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }

    private void Die()
    {
        _LevelController.instance.ForceMovement -= StepForwards;
        _LevelController.instance.ForceMovement -= KnightAI;
        _LevelController.instance.knights.Remove(this);
        Destroy(gameObject);
    }

    public override bool DealWithTrap()
    {
        health -= ((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).thing as Trap).Activate();
        if (health < 0)
        {
            Die();
        }
        return true;
    }

    public override bool DealWithKnight()
    {
        return true;
    }

    public override bool DealWithMonster()
    {
        Fight((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).agent as Monster);
        return true;
    }

    public override bool DealWithDoor()
    {
        return (_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Door).Open(this);
    }



}
