using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TerrainGen : MonoBehaviour
{
    public Tilemap tilemap;

    private GameObject player;

    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;

    [SerializeField]
    private float mapScale = 0;

    //size of tiles made with perlin noise
    private int chunkSize = 32;

    [SerializeField]
    private TerrainType[] heightTerrainTypes = null;

    [SerializeField]
    private TerrainType[] heatTerrainTypes = null;

    [SerializeField]
    private TerrainType[] moistureTerrainTypes = null;

    [SerializeField]
    private AnimationCurve moistureCurve = null;

    [SerializeField]
    private AnimationCurve heatCurve = null;

    [SerializeField]
    private Wave[] moistureWaves = null;

    [SerializeField]
    private Wave[] heatWaves = null;

    [SerializeField]
    private Wave[] heightWaves = null;

    [SerializeField]
    private Visualizationmode visualizationmode = Visualizationmode.Height;

    [SerializeField]
    private BiomeRow[] biomes = null;


    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //test chunk generation
        for (int i = -4; i < 4; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                GenerateTile(i, j);
            }
        }
    }

    void GenerateTile(int oX, int oZ)
    {
        int tileDepth = chunkSize;
        int tileWidth = tileDepth;

        //offset passed into function
        float offsetX = oX * chunkSize;
        float offsetZ = oZ * chunkSize;

        //generate heightMap using Perlin Noise
        float[,] heightMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.heightWaves);

        //generate heatMap using Perlin Noise
        float[,] heatMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.heatWaves);
        //add height values to heat map values to make higher regions colder
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                heatMap[zIndex, xIndex] += this.heatCurve.Evaluate(heightMap[zIndex, xIndex]) * heightMap[zIndex, xIndex];
            }
        }

        //generate moistureMap using Perlin Noise
        float[,] moistureMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.moistureWaves);
        //subtract height values to moisture map values to make higher regions dryer
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                heatMap[zIndex, xIndex] -= this.moistureCurve.Evaluate(heightMap[zIndex, xIndex]) * heightMap[zIndex, xIndex];
            }
        }

        //render tile(s) depending on visualization mode specified
        switch (this.visualizationmode)
        {
            case Visualizationmode.Height:
            //render height map
            RenderTiles(heightMap, (int)offsetX, (int)offsetZ);
            break;
            //render heat map
            case Visualizationmode.Heat:
            RenderTiles(heatMap, (int)offsetX, (int)offsetZ);
            break;
            //render moisture map
            case Visualizationmode.Moisture:
            RenderTiles(moistureMap, (int)offsetX, (int)offsetZ);
            break;
            //render biomes map
            case Visualizationmode.Biome:
            //build biomes from heat, height and moisture maps
            CalculateBiomes(heightMap, heatMap, moistureMap, (int)offsetX, (int)offsetZ);
            break;

            default:
            //default to height map
            RenderTiles(heightMap, (int)offsetX, (int)offsetZ);
            break;
        }

        
    }

    //method to calculate correct Tile (height, heat, moisture)
    void RenderTiles(float[,] selectedMap, int oX, int oZ)
    {
        int tileDepth = selectedMap.GetLength(0);
        int tileWidth = selectedMap.GetLength(1);

        //Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //int colorIndex = zIndex * tileWidth + xIndex;
                //parameter = height, heat, moisture
                float parameter = selectedMap[zIndex, xIndex];

                //get terrain type for given height and add it to the tilemap
                tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), GetTerrainTypeForParameter(parameter).tile);
                
            }
        }
    }

    //method to return terrainType depending on the height, heat, moisture input
    TerrainType GetTerrainTypeForParameter(float param)
    {
        TerrainType[] terrainArray = null;
        //pick tile set to use depending on visualization input
        switch (this.visualizationmode)
        {
            case Visualizationmode.Height:
            // height map
            terrainArray = heightTerrainTypes;
            break;
            // heat map
            case Visualizationmode.Heat:
            terrainArray = heatTerrainTypes;
            break;
            // moisture map
            case Visualizationmode.Moisture:
            terrainArray = moistureTerrainTypes;
            break;

            default:
            //default to height map
            terrainArray = heightTerrainTypes;
            break;
        }

        foreach(TerrainType terrainType in terrainArray)
        {
            if(param < terrainType.threshold)
            {
                //correct height
                return terrainType;
            }
        }
        //if no terrainTypes apply return the last (highest) one (sometimes perlinNoise return > 1 (or < 0))
        return terrainArray[terrainArray.Length -1];
    }

    TerrainType GetTerrainTypeForParameter(float param, TerrainType[] terrainTypes)
    {
        foreach(TerrainType terrainType in terrainTypes)
        {
            if(param < terrainType.threshold)
            {
                //correct height
                return terrainType;
            }
        }
        //if no terrainTypes apply return the last (highest) one (sometimes perlinNoise return > 1 (or < 0))
        return terrainTypes[terrainTypes.Length -1];
    }


    //method to build biome texture dependent on heat, moisture and height
    private void CalculateBiomes(float[,] heightMap, float[,] heatMap, float[,] moistureMap, int oX, int oZ)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //get height terrain type for point
                float heightValue = heightMap[zIndex, xIndex];
                TerrainType heightTerrainType = GetTerrainTypeForParameter(heightValue, heightTerrainTypes);
                //if water region render water tile (water not conform to biomes atm), else calculate correct biome Tile
                if(heightTerrainType.name != "water")
                {
                    //define biome by heat and moisture terrain types
                    float heatValue = heatMap[zIndex, xIndex];
                    TerrainType heatTerrainType = GetTerrainTypeForParameter(heatValue, heatTerrainTypes);
                    
                    float moistureValue = moistureMap[zIndex, xIndex];
                    TerrainType moistureTerrainType = GetTerrainTypeForParameter(moistureValue, moistureTerrainTypes);

                    //use biomes table to calculate correct biome for respective heat and moisture values
                    Biome biome = this.biomes [moistureTerrainType.index].biomes [heatTerrainType.index];
                    
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), biome.tile);
                }
                else
                {
                    //render normal water Tile (height value)
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), GetTerrainTypeForParameter(heightValue).tile);
                }


        
            }
        }

        
    }

}

[System.Serializable]
public class TerrainType
{
    public string name;
    //height, heat, moisture
    public float threshold;
    public Tile tile;
    public int index;
}

[System.Serializable]
public class Wave
{
    public float seed;
    public float amplitude;
    public float frequency;
}

//enum visualization mode for height, heat, moisture maps
enum Visualizationmode {Height, Heat, Moisture, Biome}

//class for biomes
[System.Serializable]
public class Biome
{
    public string name;
    public Tile tile;
}


//class for rows of biomes
[System.Serializable]
public class BiomeRow
{
     public Biome[] biomes;
}