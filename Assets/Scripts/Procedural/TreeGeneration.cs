﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    [SerializeField]
    private NoiseMapGeneration noiseMapGeneration;
    
    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private float levelScale;

    [SerializeField]
    private float neighbourRadius;

    [SerializeField]
    private GameObject treePrefab;

    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
    }

    //method to generate trees for given chunk
    public void GenerateTrees(TileData tileData)
    {
        int oX = tileData.offsetX;
        int oZ = tileData.offsetZ;
        int chunkDepth = TerrainGen.chunkSize;
        int chunkWidth = TerrainGen.chunkSize;
        int worldX = tileData.GetWorldCoordsForChunk()[0];
        int worldZ = tileData.GetWorldCoordsForChunk()[1];


        //generate Perlin Noise tree map
        float[,] treeMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(chunkDepth, chunkWidth, this.levelScale, oX, oZ, this.waves);

        for (int zIndex = 0; zIndex < chunkDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < chunkWidth; xIndex++)
            {
                //get terrain type of the coordinate
                TerrainType terrainType = tileData.chosenHeightTerrainTypes[zIndex, xIndex];

                //check if water, if so don't place tree
                if(terrainType.name != "water")
                {
                    float treeValue = treeMap[zIndex, xIndex];

                    int terrainTypeIndex = terrainType.index;

                    //compare current tree noise value to neighbours'
                    int neighborZBegin = (int)Mathf.Max (0, zIndex - this.neighbourRadius);
                    int neighborZEnd = (int)Mathf.Min (chunkDepth-1, zIndex + this.neighbourRadius);
                    int neighborXBegin = (int)Mathf.Max (0, xIndex - this.neighbourRadius);
                    int neighborXEnd = (int)Mathf.Min (chunkWidth-1, xIndex + this.neighbourRadius);
                    float maxValue = 0f;

                    for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++) {
                        for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++) {
                            float neighborValue = treeMap [neighborZ, neighborX];
                            // saves the maximum tree noise value in the radius
                            if (neighborValue >= maxValue) {
                                maxValue = neighborValue;
                            }
                        }
                    }

                    //if current tree noise value is the maximum, place tree at that location
                    if(treeValue == maxValue)
                    {
                        Vector3 treePosition = new Vector3(xIndex + worldX + 0.5f, zIndex + worldZ + 0.5f, 0);
                        GameObject tree = Instantiate(this.treePrefab, treePosition, Quaternion.identity) as GameObject;
                        //tree.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                    }

                }
            }
        }
    
    }
}
