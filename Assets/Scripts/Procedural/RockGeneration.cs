using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGeneration : MonoBehaviour
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
    private GameObject[] rockPrefabs = null;

    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
    }

    //method to generate trees for given chunk
    public void GenerateRocks(TileData tileData)
    {
        int oX = tileData.offsetX;
        int oZ = tileData.offsetZ;
        int chunkDepth = TerrainGen.chunkSize;
        int chunkWidth = TerrainGen.chunkSize;
        int worldXoffset = tileData.GetWorldCoordsForChunk()[0];
        int worldZoffset = tileData.GetWorldCoordsForChunk()[1];


        //generate Perlin Noise rock map
        float[,] rocksMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(chunkDepth, chunkWidth, this.levelScale, worldXoffset, worldZoffset, this.waves);

        for (int zIndex = 0; zIndex < chunkDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < chunkWidth; xIndex++)
            {
                //get terrain type of the coordinate
                TerrainType terrainType = tileData.chosenHeightTerrainTypes[zIndex, xIndex];

                //get biome type of the coordinate
                Biome biome = tileData.chosenBiomes[zIndex, xIndex];
                

                //check if water, if so don't place rock
                if((terrainType.name != "water"))
                {
                    float rocksValue = rocksMap[zIndex, xIndex];

                    int terrainTypeIndex = terrainType.index;

                    //compare current rock noise value to neighbours'
                    int neighborZBegin = (int)Mathf.Max (0, zIndex - this.neighbourRadius[biome.index]);
                    int neighborZEnd = (int)Mathf.Min (chunkDepth-1, zIndex + this.neighbourRadius[biome.index]);
                    int neighborXBegin = (int)Mathf.Max (0, xIndex - this.neighbourRadius[biome.index]);
                    int neighborXEnd = (int)Mathf.Min (chunkWidth-1, xIndex + this.neighbourRadius[biome.index]);
                    float maxValue = 0f;

                    for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++) {
                        for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++) {
                            float neighborValue = rocksMap [neighborZ, neighborX];
                            // saves the maximum rock noise value in the radius
                            if (neighborValue >= maxValue) {
                                maxValue = neighborValue;
                            }
                        }
                    }

                    //if current tree noise value is the maximum, place rock at that location
                    if(rocksValue == maxValue)
                    {
                        //position including placement offset
                        float x = xIndex + worldXoffset;
                        float y = zIndex + worldZoffset;
                        float xWithOffset = x + this.rockPrefabs[biome.index].GetComponent<PlacementOffset>().GetOffsetX();
                        float yWithOffset = y + this.rockPrefabs[biome.index].GetComponent<PlacementOffset>().GetOffsetY();

                        Vector3 rockPosition = new Vector3(xWithOffset, yWithOffset, 0);
                        
                        System.Random rand = new System.Random();
                        int randomInt = rand.Next(this.rockPrefabs.Length);
                        if(this.rockPrefabs.Length > randomInt)
                        {
                            
                            if(tileData.IsSpaceFree(x, y, this.rockPrefabs[biome.index]))
                            {
                                GameObject rock = Instantiate(this.rockPrefabs[biome.index], rockPosition, Quaternion.identity) as GameObject;
                                //make sure new gameObject name doesn't have (clone)
                                rock.name = this.rockPrefabs[biome.index].name;
                                //set random stage
                                int randomS = rand.Next(4);
                                rock.GetComponent<RockController>().stage = randomS;
                                RockData rd = new RockData(rock.GetComponent<RockController>());

                                tileData.AddObjectData(x, y,"Rock",rd);
                                tileData.AddObjectGO(x, y, "Rock", rock);
                            }
                        }
                        else
                        {
                            Debug.Log("ERROR-RockGeneration: Didn't find rock to generate");
                        }
                    }
                }
            }
        }
    }
}
