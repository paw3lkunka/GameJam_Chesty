using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _Tile : _GridElement, IHeapItem<_Tile>
{
    public int gCost;
    public int hCost;
    public int fCost { get => gCost + hCost; }
    public Dictionary<Agent, _Tile> lastHops;       // last-hops for specific agent while determining his best path to the taget
    private int heapIndex;

    public int HeapIndex
    {
        get => heapIndex;
        set => heapIndex = value;
    }

    public abstract bool Walkable { get; }

    public int CompareTo(_Tile tile)
    {
        int compare = this.fCost.CompareTo(tile.fCost);

        if (compare == 0)
            compare = this.hCost.CompareTo(tile.hCost);

        return (-1) * compare;          // we prefer lower costs
    }

}
