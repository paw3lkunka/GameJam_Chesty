using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class _OurLib
{
    public static Vector3 Snap(Vector3 v3)
    {
        return new Vector3(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y));
    }
    /*
    public static void SnapPosition(this MonoBehaviour mb)
    {
        mb.transform.position = Snap(mb.transform.position);
    }
    */
    public static int IntCord( this MonoBehaviour mb, char axis)
    {
        switch (axis)
        {
            case 'x':
                return Mathf.RoundToInt(mb.transform.position.x);
            case 'y':
                return Mathf.RoundToInt(mb.transform.position.y);
            case 'z':
                return Mathf.RoundToInt(mb.transform.position.z);
            default:
                throw new ArgumentException("valid arguments: 'x','y','z'");
        }
    }
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }
}
