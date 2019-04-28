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
    [SerializeField] private int greed = 5;
    [SerializeField] private int honour = 5;
    [SerializeField] private int fear = 5;

    private float distanceToPlayer = 0f;
    private float distanceToMonster = 0f;
    private float distanceToCoin = 0f;

    private List<float> monsterDistances;
    private List<float> playerDistances;

    private enum KnightStates { DoingNothing, LookingAround, ChasingPlayer, FleeingMonster, GettingCoin }
    KnightStates knightStates = KnightStates.DoingNothing;

    private void Start()
    {
        monsterDistances = new List<float>();
        playerDistances = new List<float>();
        // create a grid
        Debug.Log("Level controller instance: " + _LevelController.instance == null);
        PathFind.Grid grid = new PathFind.Grid(_LevelController.instance.knightsTilemap.GetLength(0), _LevelController.instance.knightsTilemap.GetLength(1), _LevelController.instance.knightsTilemap);

        // create source and target points
        PathFind.Point _from = new PathFind.Point((int)transform.position.x, (int)transform.position.y);
        PathFind.Point _to = new PathFind.Point((int)targets[0].transform.position.x, (int)targets[0].transform.position.y);

        // get path
        // path will either be a list of Points (x, y), or an empty list if no path is found.
        path = PathFind.Pathfinding.FindPath(grid, _from, _to);
        foreach(PathFind.Point point in path)
        {
            Debug.Log(point.x + " " + point.y);
        }

        _LevelController.instance.ForceMovement += StepForwards;
    }

    private new void Update()
    {
        base.Update();
    }

    private void KnightAI()
    {
        switch(knightStates)
        {
            case KnightStates.DoingNothing:

                break;
            case KnightStates.LookingAround:

                break;
            case KnightStates.ChasingPlayer:

                break;
            case KnightStates.FleeingMonster:

                break;
            case KnightStates.GettingCoin:

                break;
        }
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
            //monsterDistances.Add()
        }
    }

    //private float CalculateDistance()

    private void CheckForPlayer()
    {

    }

    private void CheckForMonster()
    {

    }

    private void CheckForCoin()
    {

    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }

    private void Die()
    {
        _LevelController.instance.ForceMovement -= StepForwards;
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
