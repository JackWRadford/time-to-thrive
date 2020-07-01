using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{
    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ, Wave[] waves)
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

                float noise = 0f;
                //sum of amplitudes to divide noise by to ensure noise between 0 and 1
                float normalization = 0f;
                foreach (Wave wave in waves)
                {
                    //generate noise with PerlinNoise for a wave
                    noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }

                //normalize noise value (bewteen 0 and 1)
                noise /= normalization;
                
                noiseMap [zIndex, xIndex] = noise;
            }
        }
        return noiseMap;
    }
}
