using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Level controller main class. Should not be attached manually as an component as it is instatniated automatically by Loader script.
/// </summary>
public class _LevelController : MonoBehaviour
{
    public static _LevelController instance = null;

    [SerializeField]
    public _Tile[,] tiles;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        // Uncomment if you wish the level controller to persist between the scenes
        // Note: this is probably not needed in our case
        //      DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {

        int maxX = 0, maxY = 0;

        foreach (_GridElement element in FindObjectsOfType<_GridElement>())
        {

            maxX = (int)(maxX < element.X ? element.X : maxX);
            maxY = (int)(maxY < element.Y ? element.Y : maxY);
        }

        tiles = new _Tile[maxX + 1, maxY + 1];

        foreach (_Tile tile in FindObjectsOfType<_Tile>())
        {
            if ( tiles[tile.X, tile.Y] == null)
                tiles[tile.X, tile.Y] = tile;
            else
                throw new DuplicatedTile(tiles[tile.X, tile.Y], tile);
        }

        foreach (_Entity obj in FindObjectsOfType<_Entity>())
        {
            _Tile tile = tiles[obj.X, obj.Y];
            try
            {
                if (obj is Agent && tile is Floor )
                    (tile as Floor).agent = obj as Agent;

                else if (obj is Thing && tile is Floor)
                    (tile as Floor).thing = obj as Thing;
            }
            catch (NullReferenceException)
            {
                throw new InvalidGridObjectPosition(obj);
            }
        }
    }
}
public class DuplicatedTile : Exception
{
    string name1;
    string name2;
    int x;
    int y;
    public DuplicatedTile(_GridElement existingObj, _GridElement newObj)
    {

        name1 = existingObj.name;
        name2 = newObj.name;
        x = newObj.X;
        y = newObj.Y;
    }
    public override string Message => String.Format("Cannot assign {0} on ({1},{2}) - {3} already exists here", name2, x, y, name1);
}
