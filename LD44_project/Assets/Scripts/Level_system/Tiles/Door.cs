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
    [SerializeField] private Sprite[] crackDecals;
#pragma warning restore

    [SerializeField]
    private int doorHitDurability = 3;
    [SerializeField]
    private bool isOpen = false;
    public override bool Walkable => isOpen;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isOpen ? doorOpen : doorClosed;
    }

    public bool Open(_Creature creature)
    {
        if(creature is Player)
        {
            spriteRenderer.sprite = doorOpen;
            isOpen = true;
            _LevelController.instance.monstersTilemap[(int)transform.position.x, (int)transform.position.y] = true;
            return true;
        }
        else if(creature is Knight)
        {
            if(doorHitDurability > 0)
            {
                doorHitDurability--;
                spriteRenderer.sprite = crackDecals[doorHitDurability];
                return false;
            }
            else
            {
                isOpen = true;
                _LevelController.instance.monstersTilemap[(int)transform.position.x, (int)transform.position.y] = true;
                spriteRenderer.sprite = doorDestroyed;
                return true;
            }
        }
        return false;
    }
}
