using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{
    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale)
    {
        //empty noise map
        float[,] noiseMap = new float[mapDepth, mapWidth];

        for(int zIndex = 0; zIndex < mapDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                //calculate sample indices based on coords and scale
                float sampleX = xIndex / scale;
                float sampleZ = zIndex / scale;

                //generate noise with PerlinNoise
                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                noiseMap [zIndex, xIndex] = noise;
            }
        }

        return noiseMap;
    }
}
