using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkData
{
    public int[] coordinates;
    public float[,] heightMap;
    public float[,] heatMap;
    public float[,] moistureMap;

    public TerrainType[,] chosenHeightTerrainTypes;
    public TerrainType[,] chosenHeatTerrainTypes;
    public TerrainType[,] chosenMoistureTerrainTypes;
    public Biome[,] chosenBiomes;

    //(don't need unless more efficient than calling Resources.Load on first generation of chunks)
    // public TerrainTypeData[,] chosenHeightTerrainTypes;
    // public TerrainTypeData[,] chosenHeatTerrainTypes;
    // public TerrainTypeData[,] chosenMoistureTerrainTypes;
    // public BiomeData[,] chosenBiomes;

    public ChunkData(TileData chunk)
    {
        Debug.Log("chunk just generated: " + chunk.offsetX);
        coordinates[0] = chunk.offsetX;
        coordinates[1] = chunk.offsetZ;

        heightMap = chunk.heightMap;
        heatMap = chunk.heatMap;
        moistureMap = chunk.moistureMap;

        chosenHeightTerrainTypes = chunk.chosenHeightTerrainTypes;
        chosenHeatTerrainTypes = chunk.chosenHeatTerrainTypes;
        chosenMoistureTerrainTypes = chunk.chosenMoistureTerrainTypes;

        chosenBiomes = chunk.chosenBiomes;
    }
}
