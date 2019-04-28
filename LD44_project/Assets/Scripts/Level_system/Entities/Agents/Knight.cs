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

    private void Start()
    {
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

        _LevelController.instance.MoveAgents += StepForwards;
    }

    private new void Update()
    {
        base.Update();
    }

    protected override void Translate(int x, int y)
    {
        Debug.Log("Called translate from knight!");
        StartMovement(X + x, Y + y);
    }

    public override bool DealWithTrap()
    {
        health -= ((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).thing as Trap).Activate();
        if (health < 0)
            Destroy(gameObject);
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
