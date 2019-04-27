using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _OurLib;

public class Knight : _Agent
{
#pragma warning disable
    [SerializeField]
    private float sightRange = 10f;
    [SerializeField]
    private uint hitPoints = 100;
#pragma warning restore

    public override bool DealWithTrap()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    
}
