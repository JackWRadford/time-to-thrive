using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTypeData //(don't need unless more efficient than calling Resources.Load on first generation of chunks)
{
    public string name;
    public float threshold;
    public int index;

    public TerrainTypeData(TerrainType terrainType)
    {
        this.name = terrainType.name;
        this.threshold = terrainType.threshold;
        this.index = terrainType.index;
    }
}
