using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _Tile : _GridElement
{
    public bool discovered = false;

    public SpriteRenderer fogMask;

    public abstract bool Walkable { get; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        SpriteRenderer[] potentialMask = GetComponentsInChildren<SpriteRenderer>();
        foreach (var element in potentialMask)
        {
            if (element.tag == "FogMask")
            {
                fogMask = element;
                break;
            }
        }
        base.Start();
    }
}

