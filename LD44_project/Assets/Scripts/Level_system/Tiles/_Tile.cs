using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _Tile : _GridElement
{
    // Field for checking if the tile was discovered by Fog of War algorithm
    public bool discovered = false;
    // Storing the fogMask 
    public SpriteRenderer fogMask;

    // Abstract bool for saying if the tile is walkable by default
    public abstract bool Walkable { get; }

    protected override void OnValidate()
    {
        base.OnValidate();
    }

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

