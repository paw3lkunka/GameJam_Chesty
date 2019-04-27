﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _GridElement : MonoBehaviour
{
    public int X => (int)transform.position.x;
    public int Y => (int)transform.position.y;

    private void OnValidate()
    {
        this.SnapPosition();
        if (transform.position.x < 0 || transform.position.y < 0)
            throw new InvalidGridObjectPosition(this);
    }
}
public class InvalidGridObjectPosition : Exception
{
    int x;
    int y;
    string name;

    public InvalidGridObjectPosition(MonoBehaviour obj)
    {
        x = (int)obj.transform.position.x;
        y = (int)obj.transform.position.y;
        name = obj.name;
    }

    public override string Message => String.Format("Object {0} has invalid coordinates: ({1},{2})", name, x, y);
}