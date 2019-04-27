using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level controller main class. Should not be attached manually as an component as it is instatniated automatically by Loader script.
/// </summary>
public class _LevelController : MonoBehaviour
{
    public static _LevelController instance = null;

    [SerializeField]
    private _Tile[,] levelTiles;
    [SerializeField]
    private Vector2Int levelSize;

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
        levelTiles = new _Tile[levelSize.y, levelSize.x];
    }
}
