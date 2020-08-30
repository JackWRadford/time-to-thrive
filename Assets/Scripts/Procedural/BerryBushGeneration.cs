using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBushGeneration : MonoBehaviour
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
    private GameObject[] berryBushPrefabs = null;

    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
    }

    //method to generate trees for given chunk
    public void GenerateBerryBushes(TileData tileData)
    {
        int oX = tileData.offsetX;
        int oZ = tileData.offsetZ;
        int chunkDepth = TerrainGen.chunkSize;
        int chunkWidth = TerrainGen.chunkSize;
        int worldXoffset = tileData.GetWorldCoordsForChunk()[0];
        int worldZoffset = tileData.GetWorldCoordsForChunk()[1];


        //generate Perlin Noise berry bush map
        float[,] berryBushMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(chunkDepth, chunkWidth, this.levelScale, worldXoffset, worldZoffset, this.waves);

        for (int zIndex = 0; zIndex < chunkDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < chunkWidth; xIndex++)
            {
                //get terrain type of the coordinate
                TerrainType terrainType = tileData.chosenHeightTerrainTypes[zIndex, xIndex];

                //get biome type of the coordinate
                Biome biome = tileData.chosenBiomes[zIndex, xIndex];
                

                //check if water, if so don't place berry bush
                if((terrainType.name != "water"))
                {
                    float berryBushValue = berryBushMap[zIndex, xIndex];

                    int terrainTypeIndex = terrainType.index;

                    //compare current berryBush noise value to neighbours'
                    int neighborZBegin = (int)Mathf.Max (0, zIndex - this.neighbourRadius[biome.index]);
                    int neighborZEnd = (int)Mathf.Min (chunkDepth-1, zIndex + this.neighbourRadius[biome.index]);
                    int neighborXBegin = (int)Mathf.Max (0, xIndex - this.neighbourRadius[biome.index]);
                    int neighborXEnd = (int)Mathf.Min (chunkWidth-1, xIndex + this.neighbourRadius[biome.index]);
                    float maxValue = 0f;

                    for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++) {
                        for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++) {
                            float neighborValue = berryBushMap [neighborZ, neighborX];
                            // saves the maximum berryBush noise value in the radius
                            if (neighborValue >= maxValue) {
                                maxValue = neighborValue;
                            }
                        }
                    }

                    //if current tree noise value is the maximum, place berryBush at that location
                    if(berryBushValue == maxValue)
                    {
                        //position including placement offset
                        float x = xIndex + worldXoffset;
                        float y = zIndex + worldZoffset;
                        float xWithOffset = x + this.berryBushPrefabs[biome.index].GetComponent<PlacementOffset>().GetOffsetX();
                        float yWithOffset = y + this.berryBushPrefabs[biome.index].GetComponent<PlacementOffset>().GetOffsetY();

                        Vector3 berryBushPosition = new Vector3(xWithOffset, yWithOffset, 0);
                        // GameObject tree = Instantiate(this.treePrefab[biome.index], treePosition, Quaternion.identity) as GameObject;
                        System.Random rand = new System.Random();
                        int randomInt = rand.Next(this.berryBushPrefabs.Length);
                        if(this.berryBushPrefabs.Length > randomInt)
                        {
                            
                            if(tileData.IsSpaceFree(x, y, this.berryBushPrefabs[biome.index]))
                            {
                                GameObject berryBush = Instantiate(this.berryBushPrefabs[biome.index], berryBushPosition, Quaternion.identity) as GameObject;
                                //make sure new gameObject name doesn't have (clone)
                                berryBush.name = this.berryBushPrefabs[biome.index].name;
                                BerryBushData bbd = new BerryBushData(berryBush.GetComponent<BerryBushController>());

                                tileData.AddObjectData(x, y,"BerryBush",bbd);
                                tileData.AddObjectGO(x, y, "BerryBush", berryBush);
                            }
                        }
                        else
                        {
                            Debug.Log("ERROR-BerryBushGeneration: Didn't find berry bush to generate");
                        }
                    }
                }
            }
        }
    }
}
