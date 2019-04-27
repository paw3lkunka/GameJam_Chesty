using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Diagnostics;
using static _OurLib;

public class PathFinder : MonoBehaviour
{
    private Agent agent;

    private void Start()
    {
        agent = GetComponent<Agent>();
    }

    public void FindPath(_GridElement target)               // changes value of lastHops dictionary in tile, looks like last-hop table for specific agents
    {
        _Tile startTile = LC.tiles[agent.X, agent.Y];
        _Tile targetTile = LC.tiles[target.X, target.Y];

        Heap<_Tile> openSet = new Heap<_Tile>(LC.tiles.Length);
        HashSet<_Tile> closedSet = new HashSet<_Tile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            _Tile currentTile = openSet.RemoveFirst();      // THE FUTURE IS NOW OLD MAN
            /*
            _Tile currentTile = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
                if (openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
                    currentTile = openSet[i];

            openSet.Remove(currentTile);
            */
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
                return;

            foreach (_Tile neighbour in NeighboursOf(currentTile))
            {
                if (!(neighbour.Walkable) || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, targetTile);
                if (newMovementCostToNeighbour < neighbour.gCost || !(openSet.Contains(neighbour)))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetTile);
                    neighbour.lastHops[agent] = currentTile; 

                    if (!(openSet.Contains(neighbour)))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    public List<_Tile> GetPath(_GridElement target)             // returns current path based on current lastHops for an agent
    {
        List<_Tile> path = new List<_Tile>();
        _Tile agentTile = LC.tiles[agent.X, agent.Y];
        _Tile currentTile = LC.tiles[target.X, target.Y];

        while(currentTile != agentTile)
        {
            path.Add(currentTile);
            currentTile = LC.tiles[target.X, target.Y].lastHops[agent];
        }

        path.Reverse();
        return path;
    }

    public List<_Tile> NeighboursOf(_Tile tile)
    {
        List<_Tile> neighbours = new List<_Tile>();

        for(int i = -1; i < 2; i++)
            for(int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                try
                {
                    neighbours.Add(LC.tiles[tile.X + i, tile.Y + j]);
                }
                catch(IndexOutOfRangeException){}
            }

        return neighbours;
    }

    private int GetDistance(_Tile tileA, _Tile tileB)
    {
        int dstX = Mathf.Abs(tileA.X - tileB.X);
        int dstY = Mathf.Abs(tileA.Y - tileB.Y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        else
            return 14 * dstX + 10 * (dstY - dstX);
    }
}
