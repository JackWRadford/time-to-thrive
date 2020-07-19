using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeData //(don't need unless more efficient than calling Resources.Load on first generation of chunks)
{
    public string name;
    public int index;

    public BiomeData(Biome biome)
    {
        this.name = biome.name;
        this.index = biome.index;
    }
}
