using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _GridElement : MonoBehaviour
{
    // Basic and very useful accesors to int casted object transforms
    // Use them instead of transform.position.x/y
    public int X => (int)transform.position.x;
    public int Y => (int)transform.position.y;

    protected virtual void Start()
    {
        // Snap object position to absolute values
        this.SnapPosition();
        // Tiles shouldn't have negative position for convenience
        if (transform.position.x < 0 || transform.position.y < 0)
            Debug.Log(new InvalidGridObjectPosition(this).Message);
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
