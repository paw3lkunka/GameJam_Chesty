using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;
using System;

public class Monster : _Agent
{

    protected override void Awake()
    {
        base.Awake();
    }

    private new void Start()
    {
        base.Start();
        // create a grid
        //PathFind.Grid grid = new PathFind.Grid(_LevelController.instance.monstersTilemap.GetLength(0), _LevelController.instance.monstersTilemap.GetLength(1), _LevelController.instance.monstersTilemap);

        //// create source and target points
        //PathFind.Point _from = new PathFind.Point((int)transform.position.x, (int)transform.position.y);
        //PathFind.Point _to = new PathFind.Point((int)currentTarget.transform.position.x, (int)currentTarget.transform.position.y);

        //// get path
        //// path will either be a list of Points (x, y), or an empty list if no path is found.
        //path = PathFind.Pathfinding.FindPath(grid, _from, _to);

        _LevelController.instance.ForceMovement += StepForwards;
    }

    private new void Update()
    {
        //List<PathFind.Point> path;
        Debug.Log("Update");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Key Down!");
            //PathFind.Point _from = new PathFind.Point((int)transform.position.x, (int)transform.position.y);
            //PathFind.Point _to = new PathFind.Point((int)target.transform.position.x, (int)target.transform.position.y);
            //path = PathFind.Pathfinding.FindPath(_LevelController.grid, _from, _to);

            //foreach (PathFind.Point point in path)
            //{
            //    Debug.Log(point.x + " " + point.y);
            //}
        }

    }

    public override void Die()
    {
        _LevelController.instance.ForceMovement -= StepForwards;
        _LevelController.instance.monsters.Remove(this);
        Destroy(gameObject);
    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }

    public override bool DealWithTrap()
    {
        return true;
    }

    public override bool DealWithKnight()
    {
        Fight((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).agent as Knight);
        return true;
    }

    public override bool DealWithMonster()
    {
        return true;
    }

    public override bool DealWithDoor()
    {
        return false;
    }

    public override bool DealWithPlayer()
    {
        return false;
    }
}
