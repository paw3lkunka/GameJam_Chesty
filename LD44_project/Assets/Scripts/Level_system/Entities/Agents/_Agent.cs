using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Agent : _Creature
{
    [SerializeField]
    protected List<_GridElement> targets;
    protected _GridElement currentTarget;

    protected new void Update()
    {
        base.Update();
    }

    protected float GetDistance(_GridElement element)
    {
        return Mathf.Pow(this.X - element.X, 2) + Mathf.Pow(this.Y - element.Y, 2);
    }

    protected _GridElement GetCurrentTarget()
    {
        int minDistanceIndex = 0;

        for (int i = 1; i < targets.Count; i++)
            if (GetDistance(targets[minDistanceIndex]) > GetDistance(targets[i]))
                minDistanceIndex = i;

        return targets[minDistanceIndex];
    }
}
