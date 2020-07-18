using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkData : MonoBehaviour
{
    public int[] coordinates;
    public float[,] heightMap;
    public float[,] heatMap;
    public float[,] moistureMap;

    public ChunkData(TileData chunk)
    {
        coordinates[0] = chunk.offsetX;
        coordinates[1] = chunk.offsetZ;

        heightMap = chunk.heightMap;
        heatMap = chunk.heatMap;
        moistureMap = chunk.moistureMap;
    }
}
