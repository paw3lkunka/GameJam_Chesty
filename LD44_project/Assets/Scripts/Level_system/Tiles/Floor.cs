using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Floor : _Tile
{
    // Three main reference fields for storing information about contents of current floor tile
    public _Agent agent;
    public Thing thing;
    public Player player;
    public int coins;
    public override bool Walkable => true;

    public void AssignEntity(_Entity entity)
    {
        if (entity is Player) player = (Player)entity;
        else if (entity is _Agent) agent = (_Agent)entity;
        else if (entity is Thing) thing = (Thing)entity;
        else throw new BadEntityException("Tried to assign " + entity.ToString() + " to " + ToString());
    }

    public void RemoveEntity(_Entity entity)
    {
        if (entity is Player) player = null;
        else if (entity is _Agent) agent = null;
        else if (entity is Thing) thing = null;
        else throw new BadEntityException("Tried to remove " + entity.ToString() + " from " + ToString());
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
}

[Serializable]
internal class BadEntityException : Exception
{
    public BadEntityException()
    {
    }

    public BadEntityException(string message) : base(message)
    {
    }

    public BadEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BadEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}