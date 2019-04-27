using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _Tile : _GridElement
{
    public int gCost;
    public int hCost;
    public int fCost { get => gCost + hCost; }
    public Dictionary<Agent, _Tile> parents;
    public abstract bool Walkable { get; }

}
