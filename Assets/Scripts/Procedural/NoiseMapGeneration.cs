using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{
    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ)
    {
        //empty noise map
        float[,] noiseMap = new float[mapDepth, mapWidth];

        for(int zIndex = 0; zIndex < mapDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                //calculate sample indices based on coords and scale and offset
                float sampleX = (xIndex + offsetX) / scale;
                float sampleZ = (zIndex + offsetZ) / scale;

                //generate noise with PerlinNoise
                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                noiseMap [zIndex, xIndex] = noise;
            }
        }
        return noiseMap;
    }
}
