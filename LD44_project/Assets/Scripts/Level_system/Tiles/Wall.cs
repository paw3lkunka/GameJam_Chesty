using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : _Tile
{
    public override bool Walkable => false;

    protected override void Start()
    {
        base.Start();
    }
}
