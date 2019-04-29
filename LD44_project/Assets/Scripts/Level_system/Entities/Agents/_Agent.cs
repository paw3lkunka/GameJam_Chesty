using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public abstract class _Agent : _Creature //, IWallet
{
    [SerializeField] protected readonly int baseHealth;
    [SerializeField] protected int health;
    [SerializeField] protected int attack;
    public int AttackPoints => attack;

    protected PathFind.Point _to;
    protected PathFind.Point _from;
    protected PathFind.Grid grid;
    protected List<PathFind.Point> path;
    private int pathProgress = 0;
    

    protected override void Start()
    {
        base.Start();
        path = new List<PathFind.Point>();
        _to = new PathFind.Point(X, Y);
        _from = new PathFind.Point(X, Y);
        grid = new PathFind.Grid(_LevelController.instance.knightsTilemap.GetLength(0), _LevelController.instance.knightsTilemap.GetLength(1), _LevelController.instance.knightsTilemap);
        path = PathFind.Pathfinding.FindPath(grid, _from, _to);
        ExtrapolatePath(path);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void StartMovement(float endPosX, float endPosY)
    {
        (_LevelController.instance.tiles[X, Y] as Floor).agent = null;
        base.StartMovement(endPosX, endPosY);
    }

    protected override void EndMovement()
    {
        (_LevelController.instance.tiles[X, Y] as Floor).agent = this;
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
        if (path != null && path.Count != 0)
        {
            MoveTowards(path[pathProgress]);
            if (pathProgress < path.Count - 1) pathProgress++;
            else
            {
                pathProgress = 0;
                RandomTileTarget();
            }
        }
        else throw new EmptyPathfindingPath();
    }

    public void MoveTowards(PathFind.Point point)
    {
        int x = point.x - (int)transform.position.x;
        int y = point.y - (int)transform.position.y;
        if(Mathf.Abs(x) > 1 || Mathf.Abs(y) > 1)
        {
            Debug.Log("Agent " + ToString() + " tried to move wrongly! Trying to fix this issue.");
            if (x > 1)
                x--;
            else if (x < -1)
                x++;

            if (y > 1)
                y--;
            else if (y < -1)
                y++;
            pathProgress--;
            if (Mathf.Abs(x) <= 1 || Mathf.Abs(y) <= 1) Debug.Log("Path succesfully fixed.");
            else throw new MoveByMoreThanOneTileException("Agent tried to move " + x + " " + y + " tiles, even after fixing.");
        }
        EvaluateMove(x, y);
    }

    public void Attack(_Agent opponent)
    {
        //opponent.health -= (int)attack / 2 + (int)attack * (baseHealth / 200);
        opponent.health -= attack;
        if (opponent.health < 0)
            opponent.Die();

    }

    public void Fight(_Agent opponent)
    {
        try
        {
            Attack(opponent);
            opponent.Attack(this);
        }
        catch (NullReferenceException) { }
    }

    protected float GetDistance(_GridElement element)
    {
        return Mathf.Pow(this.X - element.X, 2) + Mathf.Pow(this.Y - element.Y, 2);
    }

}

[Serializable]
internal class EmptyPathfindingPath : Exception
{
    public EmptyPathfindingPath()
    {
    }

    public EmptyPathfindingPath(string message) : base(message)
    {
    }

    public EmptyPathfindingPath(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected EmptyPathfindingPath(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}