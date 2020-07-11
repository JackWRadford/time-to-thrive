using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RiverGeneration : MonoBehaviour
{
    [SerializeField]
    private float heightThreshold = 0;

    [SerializeField]
    private Tile riverTile = null;

    public Tilemap tilemap = null;

    public void GenerateRivers(int tileDepth, int tileWidth, TileData tileData)
    {
        //choose origin for river if any point in tile (chunk) meets the heightThreshold
        Vector3  riverOrigin = ChooseRiverOrigin(tileDepth, tileWidth, tileData);

        //build the river starting from the origin if one is found
        BuildRiver(tileDepth, tileWidth, riverOrigin, tileData);
    }

    private Vector3 ChooseRiverOrigin(int tileDepth, int tileWidth, TileData tileData)
    {
        bool found = false;
        int randomZIndex = 0;
        int randomXIndex = 0;
        int tries = 0;

        while(!found)
        {
            //random coordinate in tile
            randomZIndex = Random.Range(0, tileDepth);
            randomXIndex = Random.Range(0, tileWidth);

            //convert to world coordinates from local chunk coordinates
            int worldXIndex = tileData.GetWorldCoordsForChunk()[0] + randomXIndex;
            int worldZIndex = tileData.GetWorldCoordsForChunk()[1] + randomZIndex;

            //if height value is greater than threshold at this point choose it as the river source
            float heightValue = tileData.heightMap[randomZIndex, randomXIndex];
            if(heightValue >= this.heightThreshold)
            {
                found = true;
                //debug
                print("found river source");
            }
            //make sure not infinite loop (???)
            else if(tries >= 1024)
            {
                found = true;
                //debug
                print("tried to find river source");
            }
            tries++;
        }
        return new Vector3(randomXIndex, randomZIndex, 0);
    }

    private void BuildRiver(int tileDepth, int tileWidth, Vector3 riverOrigin, TileData tileData)
    {
        HashSet<Vector3> visitedCoordinates = new HashSet<Vector3>();

        //river origin is the first coordinate
        Vector3 currentCoordinate = riverOrigin;
        int tries = 0;

        bool foundWater = false;
        while((!foundWater)&&(tries < 100))
        {
           //save current coordinate as visited
           visitedCoordinates.Add(currentCoordinate);

           //check if found water
           if(tileData.chosenHeightTerrainTypes[(int)currentCoordinate.x, (int)currentCoordinate.y].name == "water")
           {
               //stop if found water
               foundWater = true;
               //debug
               print("found water");
           }
           else
           {
                //find world position of tile to be changed to river
                Vector3Int riverTilePos = new Vector3Int((int)currentCoordinate.x + tileData.GetWorldCoordsForChunk()[0], (int)currentCoordinate.y + tileData.GetWorldCoordsForChunk()[1], 0);

                //change texture of tileDate to show a river
                tilemap.SetTile(riverTilePos, this.riverTile);

                //pick neightbour coordinates if they  exist
                List<Vector3> neighbours = new List<Vector3>();
                if(currentCoordinate.y > 0)
                {
                    neighbours.Add(new Vector3(currentCoordinate.x, currentCoordinate.y -1, 0));
                }
                if(currentCoordinate.y < tileDepth - 1)
                {
                    neighbours.Add(new Vector3(currentCoordinate.x, currentCoordinate.y +1, 0));
                }
                if(currentCoordinate.x > 0)
                {
                    neighbours.Add(new Vector3(currentCoordinate.x - 1, currentCoordinate.y, 0));
                }
                if(currentCoordinate.x < tileWidth - 1)
                {
                    neighbours.Add(new Vector3(currentCoordinate.x + 1, currentCoordinate.y, 0));
                }

                //find minimum neightbour not yet visited and flow to it
                float minHeight = float.MaxValue;
                Vector3 minNeighbour = new Vector3(0,0,0);
                foreach (Vector3 neighbour in neighbours)
                {
                    //if neighbour is lowest and has not been visited, save it
                    float neighbourHeight = tileData.heightMap[(int)neighbour.x, (int)neighbour.y];
                    if(neighbourHeight < minHeight && !visitedCoordinates.Contains(neighbour))
                    {
                        minHeight = neighbourHeight;
                        minNeighbour = neighbour;
                    }
                }
                //flow to lowest neighbour
                currentCoordinate = minNeighbour;
           }
           tries++;
        }
    }
}
