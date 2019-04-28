using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Agent : _Creature//, IWallet
{
    [SerializeField]
    protected List<_GridElement> targets;
    protected _GridElement currentTarget;

    [SerializeField]
    protected readonly int baseHealth;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected uint attack;

    public uint AttackPoints => attack;
    
    protected List<PathFind.Point> path;
    private int pathProgress = 0;

    protected new void Update()
    {
        base.Update();
    }
    protected override void StartMovement(float endPosX, float endPosY)
    {
        CurrentTile.agent = null;
        base.StartMovement(endPosX, endPosY);
    }

    protected override void EndMovement()
    {
        CurrentTile.agent = this;
        base.EndMovement();
    }

    public void StepForwards(int a, int b)
    {
        Debug.Log("Called step forwards");
        MoveTowards(path[pathProgress]);
        if(pathProgress < path.Count - 1) pathProgress++;
    }

    public void MoveTowards(PathFind.Point point)
    {
        Debug.Log("Called move towards");
        int x = point.x - (int)transform.position.x;
        int y = point.y - (int)transform.position.y;
        //Translate(x, y);
        Move(x, y);
    }

    private bool CheckTile(int x, int y)
    {
        return _LevelController.instance.tiles[X + x, Y + y].Walkable;
    }

    public void Attack(_Agent opponent)
    {
        opponent.health -= (int)attack / 2 + (int)attack * (baseHealth / 200);
        if (opponent.health < 0)
            Destroy(opponent.gameObject);

    }

    public void Fight(_Agent opponent)
    {
        try
        {
            while (true)
            {
                Attack(opponent);
                opponent.Attack(this);
            }
        }
        catch (NullReferenceException) { }
    }

    protected float GetDistance(_GridElement element)
    {
        return Mathf.Pow(this.X - element.X, 2) + Mathf.Pow(this.Y - element.Y, 2);
    }

    protected _GridElement GetCurrentTarget()
    {
        int minDistanceIndex = 0;

        for (int i = 1; i < targets.Count; i++)
            if (GetDistance(targets[minDistanceIndex]) > GetDistance(targets[i]))
                minDistanceIndex = i;

        return targets[minDistanceIndex];
    }
    /*
    public abstract uint Coins { get; set; }
    public abstract void AddCoins();
    public abstract void SubCoins();*/
}
