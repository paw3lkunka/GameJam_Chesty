using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;

public class Monster : _Agent
{
    public override bool DealWithTrap()
    {
        return true;
    }

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
}
