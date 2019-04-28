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

    public static Animator mainAnimator;
    public Player player;
    public List<Monster> monsters;
    public List<Knight> knights;
    public _Tile[,] tiles;

    [SerializeField]
    private float turnTime = 5f;
    private float elapsedTime = 0f;
    public event Action<int, int> ForceMovement;

    // Bool tilemaps for storing the pass-through block information
    public bool[,] knightsTilemap;
    public bool[,] monstersTilemap;
    // Temporarily hidden - as it should be generated in the respectful entities
    // public static PathFind.Grid grid;


    private IEnumerator timerCoroutine;

    private Vector2 currentFrameAxis;
    private Vector2 previousFrameAxis;

    public int stage = 1;

    private void Awake()
    {
        // ==========================
        // Singleton initialization
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // ==========================

        ForceMovement += (int a, int b) => Debug.Log("Event aaaaa");

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mainAnimator = player.GetComponent<Animator>();
        StartTimer();

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
            if (tiles[tile.X, tile.Y] == null)
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
                if (obj is _Agent && tile is Floor)
                {
                    (tile as Floor).agent = obj as _Agent;

                    if (obj is Knight)
                    {
                        knights.Add(obj as Knight);
                    }
                    else if (obj is Monster)
                    {
                        monsters.Add(obj as Monster);
                    }
                }

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

    public void Update()
    {
        currentFrameAxis.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if ( WereMovementKeysJustPressed() && !_LevelController.instance.player.isMoving)
        {
            (int h, int v) arg = (0, 0);
            StopTimer();
                 if (currentFrameAxis.x > 0) arg = (1, 0);  //player.MoveHoriz(1);
            else if (currentFrameAxis.x < 0) arg = (-1, 0); //player.MoveHoriz(-1);
            else if (currentFrameAxis.y > 0) arg = (0, 1);  //player.MoveVert(1);
            else if (currentFrameAxis.y < 0) arg = (0, -1); //player.MoveVert(-1);
            ForceMovement(arg.h, arg.v);
            StartTimer();
        }

        previousFrameAxis.Set(currentFrameAxis.x, currentFrameAxis.y);

    }

    private bool AreMovementKeysDownThisFrame()
    {
        return (currentFrameAxis.x != 0 || currentFrameAxis.y != 0);
    }

    private bool WereMovementKeysJustPressed()
    {
        return (currentFrameAxis.x != previousFrameAxis.x || currentFrameAxis.y != previousFrameAxis.y) && (currentFrameAxis.x != 0 || currentFrameAxis.y != 0);
    }

    private bool AreMovementKeysUpThisFrame()
    {
        return (currentFrameAxis.x != previousFrameAxis.x || currentFrameAxis.y != previousFrameAxis.y) && (currentFrameAxis.x == 0 || currentFrameAxis.y == 0);
    }

    private void StartTimer()
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = GlobalTimer();
            StartCoroutine(timerCoroutine);
        }
    }

    private void StopTimer()
    {
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator GlobalTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(turnTime);
            ForceMovement(0, 0);
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
