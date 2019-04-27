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

    public GameObject player;
    public _Tile[,] tiles;

    //public event Action<D>

    // Bool tilemaps for storing the pass-through block information
    public bool[,] knightsTilemap;
    public bool[,] monstersTilemap;
    // Temporarily hidden - as it should be generated in the respectful entities
    // public static PathFind.Grid grid;

    private void Awake()
    {
        // ==========================
        // Singleton initialization
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        // ==========================
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Generating current level bounds dynamically **********************
        int maxX = 0, maxY = 0;

        foreach (_GridElement element in FindObjectsOfType<_GridElement>())
        {
            maxX = (int)(maxX < element.X ? element.X : maxX);
            maxY = (int)(maxY < element.Y ? element.Y : maxY);
        }
        // *****************************************************************

        // Array storing every tile on the map
        tiles = new _Tile[maxX + 1, maxY + 1];

        // Filling the tiles array *****************************************
        foreach (_Tile tile in FindObjectsOfType<_Tile>())
        {
            if ( tiles[tile.X, tile.Y] == null)
                tiles[tile.X, tile.Y] = tile;
            else
                throw new DuplicatedTile(tiles[tile.X, tile.Y], tile);
        }
        // ******************************************************************

        // Filling inital tile informations (entities) **********************
        foreach (_Entity obj in FindObjectsOfType<_Entity>())
        {
            _Tile tile = tiles[obj.X, obj.Y];
            try
            {
                if (obj is _Agent && tile is Floor )
                    (tile as Floor).agent = obj as _Agent;

                else if (obj is Thing && tile is Floor)
                    (tile as Floor).thing = obj as Thing;
            }
            catch (NullReferenceException)
            {
                throw new InvalidGridObjectPosition(obj);
            }
        }
        // ********************************************************************

        // Creating knights and monsters arrays
        knightsTilemap = new bool[maxX + 1, maxY + 1];
        monstersTilemap = new bool[maxX + 1, maxY + 1];

        for (int i = 0; i < knightsTilemap.GetLength(0); i++)
        {
            for (int j = 0; j < knightsTilemap.GetLength(1); j++)
            {
                // Getting info from tile data
                if (tiles[i, j] is Wall)
                {
                    knightsTilemap[i, j] = false;
                    monstersTilemap[i, j] = false;
                }
                else if (tiles[i, j] is Floor)
                {
                    knightsTilemap[i, j] = true;
                    monstersTilemap[i, j] = true;
                }
                else if (tiles[i, j] is Door)
                {
                    knightsTilemap[i, j] = true;
                    monstersTilemap[i, j] = (tiles[i, j] as Door).Walkable;
                }
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
