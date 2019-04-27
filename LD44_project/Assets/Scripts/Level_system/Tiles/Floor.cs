using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : _Tile
{
    // Three main reference fields for storing information about contents of current floor tile
    private Agent agent;
    private Thing thing;
    private uint coin;
}
