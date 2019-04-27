using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;
using System;

public class Monster : _Agent
{
    private void Start()
    {
        
    }

    private new void Update()
    {
        //List<PathFind.Point> path;
        Debug.Log("Update");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Key Down!");
            PathFind.Point _from = new PathFind.Point((int)transform.position.x, (int)transform.position.y);
            PathFind.Point _to = new PathFind.Point((int)target.transform.position.x, (int)target.transform.position.y);
            //path = PathFind.Pathfinding.FindPath(_LevelController.grid, _from, _to);

            //foreach (PathFind.Point point in path)
            //{
            //    Debug.Log(point.x + " " + point.y);
            //}
        }

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
}
