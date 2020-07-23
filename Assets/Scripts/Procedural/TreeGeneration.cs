using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    [SerializeField]
    private NoiseMapGeneration noiseMapGeneration;
    
    [SerializeField]
    private Wave[] waves = null;

    [SerializeField]
    private float levelScale = 0;

    [SerializeField]
    private float[] neighbourRadius = null;

    [SerializeField]
    private GameObject[] treePrefab = null;

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
        int worldXoffset = tileData.GetWorldCoordsForChunk()[0];
        int worldZoffset = tileData.GetWorldCoordsForChunk()[1];


        //generate Perlin Noise tree map
        float[,] treeMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(chunkDepth, chunkWidth, this.levelScale, worldXoffset, worldZoffset, this.waves);

        for (int zIndex = 0; zIndex < chunkDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < chunkWidth; xIndex++)
            {
                //get terrain type of the coordinate
                TerrainType terrainType = tileData.chosenHeightTerrainTypes[zIndex, xIndex];

                //get biome type of the coordinate
                Biome biome = tileData.chosenBiomes[zIndex, xIndex];
                

                //check if water, if so don't place tree
                if((terrainType.name != "water"))
                {
                    float treeValue = treeMap[zIndex, xIndex];

                    int terrainTypeIndex = terrainType.index;

                    //compare current tree noise value to neighbours'
                    int neighborZBegin = (int)Mathf.Max (0, zIndex - this.neighbourRadius[biome.index]);
                    int neighborZEnd = (int)Mathf.Min (chunkDepth-1, zIndex + this.neighbourRadius[biome.index]);
                    int neighborXBegin = (int)Mathf.Max (0, xIndex - this.neighbourRadius[biome.index]);
                    int neighborXEnd = (int)Mathf.Min (chunkWidth-1, xIndex + this.neighbourRadius[biome.index]);
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
                        float x = xIndex + worldXoffset + 0.5f;
                        float y = zIndex + worldZoffset + 0.5f;
                        Vector3 treePosition = new Vector3(x, y, 0);
                        GameObject tree = Instantiate(this.treePrefab[biome.index], treePosition, Quaternion.identity) as GameObject;
                        //make sure new gameObject name doesn't have (clone)
                        tree.name = this.treePrefab[biome.index].name;
                        TreeData td = new TreeData(tree.GetComponent<TallTree>());
                        tileData.AddObject(x,y,"Tree",td);
                    }

                }
            }
        }
    
    }
}
