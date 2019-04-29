using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Trap : Thing
{
    [SerializeField] private Sprite trapArmed;
    [SerializeField] private Sprite trapDisarmed;
    [SerializeField] private int damage = 10;

    public int Damage => damage;

    private SpriteRenderer spriteRenderer;

    public bool isArmed = true;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isArmed ? trapArmed : trapDisarmed;
    }

    //returns damage on first activation, then 0;
    public int Activate()
    {
        if (isArmed)
        {
            isArmed = false;
            spriteRenderer.sprite = trapDisarmed;
            return damage;
        }
        else
            return 0;
    }
}
