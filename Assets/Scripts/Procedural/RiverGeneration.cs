using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverGeneration : MonoBehaviour
{
    [SerializeField]
    private float heightThreshold;

    [SerializeField]
    private Color riverColor;

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
            }
            //make sure not infinite loop (???)
            else if(tries >= 1024)
            {
                found = true;
            }
            tries++;
        }
        return new Vector3(randomXIndex, randomZIndex, 0);
    }

    private void BuildRiver(int tileDepth, int tileWidth, Vector3 riverOrigin, TileData tileData)
    {
        
    }
}
