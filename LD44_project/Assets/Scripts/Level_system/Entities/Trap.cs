using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Thing
{
    private SpriteRenderer renderer = null;
    public Sprite armedS;
    public Sprite disarmedS;

    public bool armed = true;
    [SerializeField]
    private int damage;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = armedS;
    }

    //returns damage on first activation, then 0;
    public int Activate()
    {
        if (armed)
        {
            armed = false;
            renderer.sprite = disarmedS;
            return damage;
        }
        else
            return 0;
    }
}
