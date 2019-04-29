using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(Animator))]
public abstract class _Creature : _Entity
{
#pragma warning disable

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private AnimationCurve movementCurve;

#pragma warning restore

    protected (bool state, int x, int y) coinsAutoCollect;
    public int money = 1;
    public bool isMoving = false;
    protected bool monsterOutcome = true, knightOutcome = true, trapOutcome = true;
    protected Vector2Int movementVector;

    private Vector2 startPos;
    private Vector2 endPos;
    private float startTime;
    
    public Animator animator;

    // Abstract methods for derived classes to implement
    protected abstract void Translate(int x, int y);
    public abstract bool DealWithPlayer();
    public abstract bool DealWithTrap(Trap trap);
    public abstract bool DealWithKnight(Knight knight);
    public abstract bool DealWithMonster(Monster monster);
    public abstract bool DealWithDoor(Door door);
    public abstract void Die();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        endPos = new Vector2();
        startPos = new Vector2();
        movementVector = new Vector2Int();
    }

    /// <summary>
    /// Move freely. Positive is up and right.
    /// </summary>
    /// <param name="x">Right = 1; Left = -1</param>
    /// <param name="y">Up = 1; Right = -1</param>
    protected void EvaluateMove(int x, int y)
    {
        // Checking an agent wants to move. Agent shouldn't want to move more than one tile at a time, so here we check for movement validity
        if (x > 1 || x < -1 || y > 1 || y < -1)
            throw new MoveByMoreThanOneTileException(String.Format("Can only move by 1 tile at a time! (Tried moving {0} tiles horizontal and {1} tiles vertical)", x, y));

        // Setting up the movement vector, for other functions to know which way we're heading at the moment
        movementVector.Set(x, y);

        // Temporary animation solution, remove type checking for other creatures to work
        if (this is Player || this is Knight)
        {
            if (y > 0)
                animator.SetTrigger("moveUp");
            else if (y < 0)
                animator.SetTrigger("moveDown");
            else if (x > 0)
                animator.SetTrigger("moveRight");
            else if (x < 0)
                animator.SetTrigger("moveLeft");
        }


        // Get the target tile reference
        _Tile targetTile = _LevelController.instance.tiles[X + x, Y + y];

        monsterOutcome = true;
        trapOutcome = true;
        knightOutcome = true;

        if (targetTile is Floor)
        {
            Floor floor = targetTile as Floor;
            // Deal with things in way of our creature
            if ( (floor.thing as Trap)?.isArmed ?? false) // if floor.thing is not null it returns .armed property, if it is null, it returns null (and doesn't throw NullReference) then null is treated as false (using ?? operator)
            {
                trapOutcome = DealWithTrap(floor.thing as Trap);
            }
            if (floor.agent is Monster)
            {
                monsterOutcome = DealWithMonster(floor.agent as Monster);
            }
            else if (floor.agent is Knight)
            {
                knightOutcome = DealWithKnight(floor.agent as Knight);
            }
            
        }
        else if(targetTile is Door)
        {
            if ((targetTile as Door).Walkable)
            {
                movementVector.Set(2 * x, 2 * y);
            }
            else if (DealWithDoor(targetTile as Door) == true)
            {
                movementVector.Set(2 * x, 2 * y);
            }
            else
            {
                movementVector.Set(0, 0);
            }
            //else throw new NotImplementedException("Bad operation on door executed by " + ToString());
        }
        else
        {
            movementVector.Set(0, 0);
        }

    }

    protected void ExecuteMove()
    {
        if (monsterOutcome && knightOutcome && trapOutcome)
        {
            Debug.Log("Executing succesful move from " + ToString());
            Translate(movementVector.x, movementVector.y);
        }
    }

    /// <summary>
    /// Initiates linear interpolation of movement from current position to specified
    /// </summary>
    protected virtual void StartMovement(float endPosX, float endPosY)
    {
        if(!isMoving)
        {
            startPos.Set(X, Y);
            endPos.Set(endPosX, endPosY);
            startTime = Time.time;
            isMoving = true;
        }
    }

    protected virtual void EndMovement()
    {
        isMoving = false;

        if (coinsAutoCollect.state)
            CollectCoins();
        if (coinsAutoCollect.x != X || coinsAutoCollect.y != Y)
            coinsAutoCollect.state = true;
    }

    /// <summary>
    /// Handles interpolated movement over time specified by movementSpeed
    /// </summary>
    protected virtual void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.Lerp(startPos, endPos, movementCurve.Evaluate(Time.time - startTime) * movementSpeed);
            if ((Vector2)transform.position == endPos)
            {
                EndMovement();
            }
        }
    }

#region Money



    public virtual void DropCoins(int amount)
    {
        if (amount <= money)
        {
            (_LevelController.instance.tiles[X, Y] as Floor).coins += amount;
            money -= amount;

            // Fast animation hack
            // if (this is Player) _LevelController.mainAnimator.SetTrigger("coinLoss");
        }
    }

    public void CollectCoins()
    {
        try
        {
            ref int tileCoins = ref (_LevelController.instance.tiles[X, Y] as Floor).coins;
            money += tileCoins;
            tileCoins = 0;

            // Fast animation hack
            // if (this is Player && tileCoins != 0) _LevelController.mainAnimator.SetTrigger("coinGain");
        }
        catch (NullReferenceException) { }
    }


#endregion
}

[Serializable]
internal class MoveByMoreThanOneTileException : Exception
{
    public MoveByMoreThanOneTileException()
    {
    }

    public MoveByMoreThanOneTileException(string message) : base(message)
    {
    }

    public MoveByMoreThanOneTileException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected MoveByMoreThanOneTileException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}