using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _Tile : _GridElement
{
    public abstract bool Walkable { get; }

}
