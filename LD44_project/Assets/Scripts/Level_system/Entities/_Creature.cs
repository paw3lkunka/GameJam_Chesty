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
        try
        {
            ref uint tileCoins = ref (_LevelController.instance.tiles[X, Y] as Floor).coins;
            money += tileCoins;
            tileCoins = 0;
        }
        catch (NullReferenceException) { }
    }
    #endregion

#pragma warning disable
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private AnimationCurve movementCurve;
#pragma warning restore

    public static bool repeatMovement = true;

    private bool isMoving = false;
    private float startTime;
    
    private Vector2 startPos;
    private Vector2 endPos;

    protected Vector2Int movementVector;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        endPos = new Vector2();
        startPos = new Vector2();
        movementVector = new Vector2Int();
    }

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
    protected void Move(int x, int y)
    {
        if (x > 1 || x < -1 || y > 1 || y < -1)
            throw new ArgumentOutOfRangeException(String.Format("Can only move by 1 tile at a time! (Tried moving {0} tiles horizontal and {1} tiles vertical)", x, y));

        movementVector.Set(x, y);
        // Get the target tile reference
        _Tile targetTile = _LevelController.instance.tiles[X + x, Y + y];

        if(targetTile is Floor)
        {
            Floor floor = targetTile as Floor;
            bool monsterOutcome = true, knightOutcome = true, trapOutcome = true;
            // if floor.thing is not null it returns .armed property, if it is null, it returns null (and doesn't throw NullReference) then null is treated as false (using ?? operator)
            if ( (floor.thing as Trap)?.armed ?? false ) 
            {
                trapOutcome = DealWithTrap();
            }
            if (floor.agent is Monster)
            {
                monsterOutcome = DealWithMonster();
            }
            else if (floor.agent is Knight)
            {
                knightOutcome = DealWithKnight();
            }
            // Make decision if creature should move
            if(monsterOutcome && knightOutcome && trapOutcome)
                Translate(x, y);
        }
        else if(targetTile is Door)
        {
            if ((targetTile as Door).Walkable)
                Translate(2 * x, 2 * y);
            else if (DealWithDoor())
                Translate(2 * x, 2 * y);
        }

    }

    protected abstract void Translate(int x, int y);
    public abstract bool DealWithTrap();
    public abstract bool DealWithKnight();
    public abstract bool DealWithMonster();
    public abstract bool DealWithDoor();

    protected void StartMovement(float endPosX, float endPosY)
    {
        startPos = transform.position;
        endPos.Set(endPosX, endPosY);
        startTime = Time.time;
        repeatMovement = false;
        isMoving = true;
        animator.SetBool("isMoving", true);
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
                animator.SetBool("isMoving", false);
            }
        }
    }
}
