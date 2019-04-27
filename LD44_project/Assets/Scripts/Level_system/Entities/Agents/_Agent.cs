using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _Agent : _Creature
{
    [SerializeField]
    protected Floor target;

    [SerializeField]
    protected readonly int baseHealth;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected uint attack;

    public uint AttackPoints => attack;



    protected new void Update()
    {
        base.Update();
    }

    public void Attack( _Agent opponent)
    {
        opponent.health -= (int)attack / 2 + (int)attack * (baseHealth / 200);
        if (opponent.health < 0)
            Destroy(opponent.gameObject);

    }

    public void Fight( _Agent opponent)
    {
        try
        {
            while(true)
            {
                Attack(opponent);
                opponent.Attack(this);
            }
        }
        catch( NullReferenceException) { }
    }
}
