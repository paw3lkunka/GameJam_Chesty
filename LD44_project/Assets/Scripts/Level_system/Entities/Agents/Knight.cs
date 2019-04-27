using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;

public class Knight : _Agent
{
#pragma warning disable
    [SerializeField]
    private float sightRange = 10f;
#pragma warning restore


    void Start()
    {

    }

    protected override void Translate(int x, int y)
    {
        StartMovement(X + x, Y + y);
    }
    public override bool DealWithTrap()
    {
        health -= ((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).thing as Trap).Activate();
        if (health < 0)
            Destroy(gameObject);
        return true;
    }

    public override bool DealWithKnight()
    {
        return true;
    }

    public override bool DealWithMonster()
    {
        Fight((_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Floor).agent as Monster);
        return true;
    }

    public override bool DealWithDoor()
    {
        return (_LevelController.instance.tiles[X + movementVector.x, Y + movementVector.y] as Door).Open(this);
    }



}
