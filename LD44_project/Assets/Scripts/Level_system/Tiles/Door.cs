using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Door : _Tile
{
#pragma warning disable
    [SerializeField] private Sprite doorClosed;
    [SerializeField] private Sprite doorOpen;
    [SerializeField] private Sprite doorDestroyed;
    [SerializeField] [Tooltip("First should be the most damaged")]
                     private Sprite[] crackDecals;
#pragma warning restore

    [SerializeField]
    private int doorHitDurability = 3;
    [SerializeField]
    private bool isOpen = false;
    public override bool Walkable => isOpen;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        if (crackDecals.Length != doorHitDurability)
            throw new TooLittleCrackDecalSprites();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isOpen ? doorOpen : doorClosed;
    }

    public bool Open(_Creature creature)
    {
        if(creature is Player)
        {
            spriteRenderer.sprite = doorOpen;
            isOpen = true;
            _LevelController.instance.monstersTilemap[X, Y] = true;
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
                _LevelController.instance.monstersTilemap[X, Y] = true;
                spriteRenderer.sprite = doorDestroyed;
                return true;
            }
        }
        return false;
    }
}

[Serializable]
internal class TooLittleCrackDecalSprites : Exception
{
    public TooLittleCrackDecalSprites()
    {
    }

    public TooLittleCrackDecalSprites(string message) : base(message)
    {
    }

    public TooLittleCrackDecalSprites(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TooLittleCrackDecalSprites(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}