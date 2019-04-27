using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Agent : _Creature
{
    [SerializeField]
    protected Floor target;

    protected new void Update()
    {
        base.Update();
    }
}
