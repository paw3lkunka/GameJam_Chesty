using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Door : _Tile
{
#pragma warning disable
    [SerializeField] private Sprite doorClosed;
    [SerializeField] private Sprite doorOpen;
    [SerializeField] private Sprite doorDestroyed;
#pragma warning restore

    [SerializeField]
    private int doorHitDurability = 3;
    private bool isOpen = false;
    public override bool Walkable => isOpen;

    public void Open(_Creature creature)
    {
        if(creature is Player)
        {
            GetComponent<SpriteRenderer>().sprite = doorOpen;
            isOpen = true;
        }
        // Add knight here
    }
}
