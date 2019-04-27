using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class _Creature : _Entity
{
    #region Money
    [SerializeField]
    protected uint money = 20;
    public virtual void DropCoins(uint amount)
    {
        if ( amount <= money)
        {
            (_LevelController.instance.tiles[X, Y] as Floor).coins += amount;
            money -= amount;

        }
    }

    public void CollectCoins()
    {
        ref uint tileCoins = ref (_LevelController.instance.tiles[X, Y] as Floor).coins;
        money += tileCoins;
        tileCoins = 0;
    }
    #endregion

#pragma warning disable
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private AnimationCurve movementCurve;
#pragma warning restore

    public static bool repeatMovement = false;

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
    private void Move(int x, int y)
    {
        Debug.Log("Invoked Move!");
        // Temporarily we can move only by one tile at a time. This can be easily changed if needed.
        if (x > 1 || x < -1 || y > 1 || y < -1) throw new ArgumentOutOfRangeException(String.Format("Can only move by 1 tile at a time! (Tried moving {0} tiles horizontal and {1} tiles vertical)", x, y));

        if (!isMoving)
        {
            repeatMovement = false;
            endPos = new Vector2((int)transform.position.x + x, (int)transform.position.y + y);
        }

        try
        {
            _Tile tempTile = _LevelController.instance.tiles[(int)endPos.x, (int)endPos.y];
            // Tile at offset position must be floor, otherwise do nothing (this should change as here the enemies move event should be invoked)
            if (tempTile is Floor && tempTile.Walkable && !isMoving) // Change _LevelController to LC later
            {
                StartMovement();
            }
            else if (tempTile is Door && !tempTile.Walkable && !isMoving)
            {
                Debug.Log("Trying to open");
                (tempTile as Door).Open(this);
            }
            else if (tempTile is Door && tempTile.Walkable && !isMoving)
            {
                endPos = new Vector2(tempTile.X + x, tempTile.Y + y);
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

    private void StartMovement()
    {
        startPos = transform.position;
        startTime = Time.time;
        isMoving = true;
    }

    protected void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.Lerp(startPos, endPos, movementCurve.Evaluate(Time.time - startTime) * movementSpeed);
            if ((Vector2)transform.position == endPos)
            {
                repeatMovement = true;
                isMoving = false;
            }
        }
    }
}



//dupa dupa dupa dupad