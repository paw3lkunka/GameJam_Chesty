using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;
using System;

public abstract class _Entity : _GridElement
{
    public Floor CurrentTile
    {
        get => _LevelController.instance.tiles[X, Y] as Floor;
        set
        {
            Floor f = _LevelController.instance.tiles[X, Y] as Floor;
            f = value;
        }
    }

    // may return null
    public T CurrentTilePlus<T>( int x, int y) where T: _Tile{
        return _LevelController.instance.tiles[X + x, Y + y] as T;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
}