using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class _Creature : _Entity
{
#pragma warning disable
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private AnimationCurve movementCurve;
#pragma warning restore

    private bool isMoving = false;
    private float startTime;
    
    private Vector2 startPos;
    private Vector2 endPos;

    /// <summary>
    /// Up is positive, down is negative
    /// </summary>
    public void MoveVert(int n)
    {
        Move(0, n);
    }

    /// <summary>
    /// Right is positive, left is negative
    /// </summary>
    public void MoveHoriz(int n)
    {
        Move(n, 0);
    }

    /// <summary>
    /// Move freely. Positive is up and right.
    /// </summary>
    /// <param name="x">Right = 1; Left = -1</param>
    /// <param name="y">Up = 1; Right = -1</param>
    public void Move(int x, int y)
    {
        // Temporarily we can move only by one tile at a time. This can be easily changed if needed.
        if (x > 1 || x < -1 || y > 1 || y < -1) throw new ArgumentOutOfRangeException(String.Format("Can only move by 1 tile at a time! (Tried moving {0} tiles horizontal and {1} tiles vertical)", x, y));

        if (!isMoving)
        {
            endPos = new Vector2((int)transform.position.x + x, (int)transform.position.y + y);
        }

        try
        {
            // Tile at offset position must be floor, otherwise do nothing (this should change as here the enemies move event should be invoked)
            if (_LevelController.instance.tiles[(int)endPos.x, (int)endPos.y].Walkable && !isMoving) // Change _LevelController to LC later
            {
                startPos = transform.position;
                startTime = Time.time;
                isMoving = true;
            }
            else
            { 
                return; // Here maybe we should return some info about the failure of movement
            }
        }
        catch (IndexOutOfRangeException)
        {
            // Do nothing if index is out of range
        }
    }

    protected void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.Lerp(startPos, endPos, movementCurve.Evaluate(Time.time - startTime) * movementSpeed);
            if ((Vector2)transform.position == endPos) isMoving = false;
        }
    }
}
