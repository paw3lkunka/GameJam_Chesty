using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static _OurLib;

public class FOV : MonoBehaviour
{
    public uint viewRadius = 5;

    [Range(0f, 1f)]
    public float visible = 1f;
    [Range(0f, 1f)]
    public float remembered = 0.5f;
    [Range(0f, 1f)]
    public float undiscovered = 0.02f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _Tile[,] tiles = _LevelController.instance.tiles;

        for ( int i=0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                _Tile tile = tiles[i, j];

                if (tile == null)
                    continue;

                float scale = 0;

                if (Vector2.Distance(Snap(transform.position), tile.transform.position) < viewRadius)
                {
                    scale = visible;
                    tile.discovered = true;
                }
                else if (tile.discovered)
                    scale = remembered;
                else
                    scale = undiscovered;
                

                SpriteRenderer[] renderers = tiles[i, j].GetComponentsInChildren<SpriteRenderer>();

                foreach( var renderer in renderers)
                {
                    renderer.color = new Color(1f, 1f, 1f, scale);
                }

                

                try
                {
                    Floor floor = tiles[i, j] as Floor;
                    floor.agent.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, scale != visible ? 0f : visible );
                    floor.thing.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, scale);

                }
                catch (Exception) { }
            }
        }
    }
}
