using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Agent : _Creature//, IWallet
{
    [SerializeField]
    protected _GridElement currentTarget;

    [SerializeField]
    protected readonly int baseHealth;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected uint attack;

    protected PathFind.Point _to;
    protected PathFind.Point _from;
    protected PathFind.Grid grid;

    public uint AttackPoints => attack;
    
    protected List<PathFind.Point> path;
    private int pathProgress = 0;

    protected void Start()
    {
        path = new List<PathFind.Point>();
        _to = new PathFind.Point(X, Y);
        _from = new PathFind.Point(X, Y);
        grid = new PathFind.Grid(_LevelController.instance.knightsTilemap.GetLength(0), _LevelController.instance.knightsTilemap.GetLength(1), _LevelController.instance.knightsTilemap);
        path = PathFind.Pathfinding.FindPath(grid, _from, _to);
        ExtrapolatePath(path);
    }

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

    protected void RandomTileTarget()
    {
        _from.Set(X, Y);
        while (true)
        {
            _Tile[,] tempTiles = _LevelController.instance.tiles;
            _Tile targetTile = tempTiles[UnityEngine.Random.Range(0, tempTiles.GetLength(0)), UnityEngine.Random.Range(0, tempTiles.GetLength(1))];
            if (targetTile is Floor)
            {
                _to.Set(targetTile.X, targetTile.Y);
                path = PathFind.Pathfinding.FindPath(grid, _from, _to);
                if (path.Count != 0)
                {
                    ExtrapolatePath(path);
                    break;
                }
            }
        }
    }

    protected void ExtrapolatePath(List<PathFind.Point> path)
    {
        for (int i = 1; i < path.Count; i++)
        {
            if (path[i].x != path[i - 1].x && path[i].y != path[i - 1].y)
            {
                if (_LevelController.instance.tiles[path[i].x, path[i - 1].y] is Floor)
                    path.Insert(i, new PathFind.Point(path[i].x, path[i - 1].y));
                else
                    path.Insert(i, new PathFind.Point(path[i - 1].x, path[i].y));

            }
        }
    }

    public void StepForwards(int a, int b)
    {
        Debug.Log("Called step forwards");
        if(path != null)
        {
            MoveTowards(path[pathProgress]);
            if (pathProgress < path.Count - 1) pathProgress++;
            else
            {
                pathProgress = 0;
                RandomTileTarget();
            }
        }
    }

    public void MoveTowards(PathFind.Point point)
    {
        Debug.Log("Called move towards");
        int x = point.x - (int)transform.position.x;
        int y = point.y - (int)transform.position.y;
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

    /*
    public abstract uint Coins { get; set; }
    public abstract void AddCoins();
    public abstract void SubCoins();*/
}
