using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : _Creature
{
    private Vector2 currentFrameAxis;
    //private Vector2 previousFrameAxis;

    private new void Update()
    {
        base.Update();
        currentFrameAxis.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if( (currentFrameAxis.x != 0 || currentFrameAxis.y != 0))
        {
            if (currentFrameAxis.x > 0) MoveHoriz(1);
            else if (currentFrameAxis.x < 0) MoveHoriz(-1);
            else if (currentFrameAxis.y > 0) MoveVert(1);
            else if (currentFrameAxis.y < 0) MoveVert(-1);
        }

        //previousFrameAxis.Set(currentFrameAxis.x, currentFrameAxis.y);
    }
}
