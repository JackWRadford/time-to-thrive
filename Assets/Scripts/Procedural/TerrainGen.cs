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
    public static int chunkSize = 32;

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

    [SerializeField]
    private TreeGeneration treeGeneration;

    [SerializeField]
    private RiverGeneration riverGeneration;

    private LevelData levelData = null;


    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
        treeGeneration = this.GetComponent<TreeGeneration>();
        riverGeneration = this.GetComponent<RiverGeneration>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //empty level data object to be filled as chunks are generated / updated
        levelData = new LevelData(chunkSize, chunkSize);

        //test chunk generation
        // for (int i = -5; i < 5; i++)
        // {
        //     for (int j = -5; j < 5; j++)
        //     {
        //         TileData tileData = GenerateTile(i, j);
        //         levelData.AddTileData(tileData, i, j);

        //         //generate trees for chunk
        //         treeGeneration.GenerateTrees(tileData);

        //         //generate rivers for chunk
        //         //riverGeneration.GenerateRivers(TerrainGen.chunkSize, TerrainGen.chunkSize, tileData);
        //     }
        // }
    }

    void Update()
    {
        //check if should generate/load more chunks depending on players position and loaded chunks
        NeedNewChunk();
    }

    //method to check if new chunk needs to be generated/ loaded
    private void NeedNewChunk()
    {
        Vector3Int playerPos = new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, 0);

        //find coordinates of chunk that player is in
        int chunkX = (int)(playerPos.x / TerrainGen.chunkSize);
        int chunkY = (int)(playerPos.y / TerrainGen.chunkSize);

        //loop through all chunk coordinates that should be loaded dependant on player's position
        for (int i = chunkX - 2; i < chunkX + 2; i++)
        {
            for (int j = chunkY -2; j < chunkY +2; j++)
            {
                if(levelData.FindChunk(i,j) != null)
                {
                    //chunk exists, load it
                    LoadChunk(i,j);
                }else{
                    //chunk doesn't exist, generate it
                    GenerateChunk(i,j);
                }
            }
        }
    }

    //method to generate new chunk at given coordinates
    private void GenerateChunk(int i, int j)
    {
        print("generate chunk:" + i.ToString() + ", " + j.ToString());
        //generate tileData (chunkData) and generate tile (chunk)
        TileData tileData = GenerateTile(i, j);
        levelData.AddTileData(tileData, i, j);

        //generate trees for chunk
        treeGeneration.GenerateTrees(tileData);
    }

    //mehtod to load saved chunk
    private void LoadChunk(int i, int j)
    {
        print("load chunk:" + i.ToString() + ", " + j.ToString());
    }

    private TileData GenerateTile(int oX, int oZ)
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

        //calculate terrainTypes for each map
        TerrainType[,] chosenHeightTerrainTypes = new TerrainType[tileDepth, tileWidth];
        TerrainType[,] chosenHeatTerrainTypes = new TerrainType[tileDepth, tileWidth];
        TerrainType[,] chosenMoistureTerrainTypes = new TerrainType[tileDepth, tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //height
                chosenHeightTerrainTypes[zIndex, xIndex] = GetTerrainTypeForParameter(heightMap[zIndex,xIndex], heightTerrainTypes);
                //heat
                chosenHeatTerrainTypes[zIndex, xIndex] = GetTerrainTypeForParameter(heatMap[zIndex,xIndex], heatTerrainTypes);
                //moisture
                chosenMoistureTerrainTypes[zIndex, xIndex] = GetTerrainTypeForParameter(moistureMap[zIndex,xIndex], moistureTerrainTypes);
            }
        }

        //build new chosenBiomes matrix for (chunk)
        Biome[,] chosenBiomes = new Biome[tileDepth, tileWidth];
        
        //render tile(s) depending on visualization mode specified
        switch (this.visualizationmode)
        {
            case Visualizationmode.Height:
            //render height map
            //RenderTiles(heightMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenHeightTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
            //render heat map
            case Visualizationmode.Heat:
            //RenderTiles(heatMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenHeatTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
            //render moisture map
            case Visualizationmode.Moisture:
            //RenderTiles(moistureMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenMoistureTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
            //render biomes map
            case Visualizationmode.Biome:
            //build biomes from heat, height and moisture maps
            //CalculateBiomes(heightMap, heatMap, moistureMap, (int)offsetX, (int)offsetZ, chosenBiomes);
            CalculateBiomes(chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes, (int)offsetX, (int)offsetZ, chosenBiomes);
            break;

            default:
            //default to height map
            //RenderTiles(heightMap, (int)offsetX, (int)offsetZ);
            RenderTiles(chosenHeightTerrainTypes, (int)offsetX, (int)offsetZ);
            break;
        }
        //build tileData for (chunk)
        TileData tileData = new TileData(heightMap, heatMap, moistureMap, chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes, chosenBiomes, oX, oZ);

        return tileData;
    }

    //method to render tiles from pre-calculated TerrainType matrix (more efficient)
    void RenderTiles(TerrainType[,] chosenTerrainTypes, int oX, int oZ)
    {
        int tileDepth = chosenTerrainTypes.GetLength(0);
        int tileWidth = chosenTerrainTypes.GetLength(1);

        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //render tile from terraintype matrix
                tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), chosenTerrainTypes[zIndex, xIndex].tile);
                
            }
        }
    }

    //method to calculate correct Tile (height, heat, moisture)
    // void RenderTiles(float[,] selectedMap, int oX, int oZ)
    // {
    //     int tileDepth = selectedMap.GetLength(0);
    //     int tileWidth = selectedMap.GetLength(1);

    //     //Color[] colorMap = new Color[tileDepth * tileWidth];
    //     for (int zIndex = 0; zIndex < tileDepth; zIndex++)
    //     {
    //         for (int xIndex = 0; xIndex < tileWidth; xIndex++)
    //         {
    //             //int colorIndex = zIndex * tileWidth + xIndex;
    //             //parameter = height, heat, moisture
    //             float parameter = selectedMap[zIndex, xIndex];

    //             //get terrain type for given height and add it to the tilemap
    //             tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), GetTerrainTypeForParameter(parameter).tile);
                
    //         }
    //     }
    // }

    //method to return terrainType depending on the height, heat, moisture input
    // TerrainType GetTerrainTypeForParameter(float param)
    // {
    //     TerrainType[] terrainArray = null;
    //     //pick tile set to use depending on visualization input
    //     switch (this.visualizationmode)
    //     {
    //         case Visualizationmode.Height:
    //         // height map
    //         terrainArray = heightTerrainTypes;
    //         break;
    //         // heat map
    //         case Visualizationmode.Heat:
    //         terrainArray = heatTerrainTypes;
    //         break;
    //         // moisture map
    //         case Visualizationmode.Moisture:
    //         terrainArray = moistureTerrainTypes;
    //         break;

    //         default:
    //         //default to height map
    //         terrainArray = heightTerrainTypes;
    //         break;
    //     }

    //     foreach(TerrainType terrainType in terrainArray)
    //     {
    //         if(param < terrainType.threshold)
    //         {
    //             //correct height
    //             return terrainType;
    //         }
    //     }
    //     //if no terrainTypes apply return the last (highest) one (sometimes perlinNoise return > 1 (or < 0))
    //     return terrainArray[terrainArray.Length -1];
    // }

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
    // private void CalculateBiomes(float[,] heightMap, float[,] heatMap, float[,] moistureMap, int oX, int oZ, Biome[,] chosenBiomes)
    // {
    //     int tileDepth = heightMap.GetLength(0);
    //     int tileWidth = heightMap.GetLength(1);

    //     for (int zIndex = 0; zIndex < tileDepth; zIndex++)
    //     {
    //         for (int xIndex = 0; xIndex < tileWidth; xIndex++)
    //         {
    //             //get height terrain type for point
    //             float heightValue = heightMap[zIndex, xIndex];
    //             TerrainType heightTerrainType = GetTerrainTypeForParameter(heightValue, heightTerrainTypes);
    //             //if water region render water tile (water not conform to biomes atm), else calculate correct biome Tile
    //             if(heightTerrainType.name != "water")
    //             {
    //                 //define biome by heat and moisture terrain types
    //                 float heatValue = heatMap[zIndex, xIndex];
    //                 TerrainType heatTerrainType = GetTerrainTypeForParameter(heatValue, heatTerrainTypes);
                    
    //                 float moistureValue = moistureMap[zIndex, xIndex];
    //                 TerrainType moistureTerrainType = GetTerrainTypeForParameter(moistureValue, moistureTerrainTypes);

    //                 //use biomes table to calculate correct biome for respective heat and moisture values
    //                 Biome biome = this.biomes [moistureTerrainType.index].biomes [heatTerrainType.index];

    //                 //save biome in chosenBiomes matrix when not water
    //                 chosenBiomes[zIndex, xIndex] = biome;
                    
    //                 tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), biome.tile);
    //             }
    //             else
    //             {
    //                 //render normal water Tile (height value)
    //                 tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), GetTerrainTypeForParameter(heightValue, heightTerrainTypes).tile);
    //             }
    //         }
    //     }
    // }

    //method to build biome texture dependent on heat, moisture and height
    private void CalculateBiomes(TerrainType[,] chosenHeightTerrainTypes, TerrainType[,] chosenHeatTerrainTypes, TerrainType[,] chosenMoistureTerrainTypes, int oX, int oZ, Biome[,] chosenBiomes)
    {
        int tileDepth = chosenHeightTerrainTypes.GetLength(0);
        int tileWidth = chosenHeightTerrainTypes.GetLength(1);

        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //if water region render water tile (water not conform to biomes atm), else calculate correct biome Tile
                if(chosenHeightTerrainTypes[zIndex,xIndex].name != "water")
                {
                    //use biomes table to calculate correct biome for respective heat and moisture values
                    Biome biome = this.biomes [chosenMoistureTerrainTypes[zIndex,xIndex].index].biomes [chosenHeatTerrainTypes[zIndex,xIndex].index];

                    //save biome in chosenBiomes matrix when not water
                    chosenBiomes[zIndex, xIndex] = biome;
                    
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), biome.tile);
                }
                else
                {
                    //render normal water Tile (height value)
                    tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), chosenHeightTerrainTypes[zIndex,xIndex].tile);
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
    public int index;
}


//class for rows of biomes
[System.Serializable]
public class BiomeRow
{
     public Biome[] biomes;
}

//class to store a (chunk's) data
public class TileData
{
    public float[,] heightMap;
    public float[,] heatMap;
    public float[,] moistureMap;
    public TerrainType[,] chosenHeightTerrainTypes;
    public TerrainType[,] chosenHeatTerrainTypes;
    public TerrainType[,] chosenMoistureTerrainTypes;
    public Biome[,] chosenBiomes;
    public int offsetX;
    public int offsetZ;


    public TileData(float[,] heightMap, float[,] heatMap, float[,] moistureMap,
    TerrainType[,] chosenHeightTerrainTypes, TerrainType[,] chosenHeatTerrainTypes, TerrainType[,] chosenMoistureTerrainTypes, Biome[,] chosenBiomes, int oX, int oZ)
    {
        this.heightMap = heightMap;
        this.heatMap = heatMap;
        this.moistureMap = moistureMap;
        this.chosenHeightTerrainTypes = chosenHeightTerrainTypes;
        this.chosenHeatTerrainTypes = chosenHeatTerrainTypes;
        this.chosenMoistureTerrainTypes = chosenMoistureTerrainTypes;
        this.chosenBiomes = chosenBiomes;
        this.offsetX = oX;
        this.offsetZ = oZ;
    }

    //method to get world coords for chunk
    public int[] GetWorldCoordsForChunk()
    {
        int[] coords = new int[2];
        coords[0] = offsetX*TerrainGen.chunkSize;
        coords[1] = offsetZ*TerrainGen.chunkSize;

        return coords;
    }

}

//class to store all the merged tiles data
public class LevelData
{
     private int tileDepthInVertices;
     private int tileWidthInVertices;

     public TileData[,] positiveTilesData;
     public TileData[,] negativeIJTilesData;
     public TileData[,] negativeITilesData;
     public TileData[,] negativeJTilesData;

     public LevelData(int tileDepthInVertices, int tileWidthInVertices)
     {
        positiveTilesData = new TileData[200,200];
        negativeIJTilesData = new TileData[200,200];
        negativeITilesData = new TileData[200,200];
        negativeJTilesData = new TileData[200,200];

        this.tileDepthInVertices = tileDepthInVertices;
        this.tileWidthInVertices = tileWidthInVertices;
     }

     //method to convert coordinates
    //  public TileCoordinate ConvertToTileCoordinate(int zIndex, int xIndex)
    //  {
    //     int tileZIndex = (int)Mathf.Floor((float)zIndex/(float)this.tileDepthInVertices);
    //     int tileXIndex = (int)Mathf.Floor((float)xIndex/(float)this.tileWidthInVertices);

    //     int coordinateZIndex = (zIndex % this.tileDepthInVertices);
    //     int coordinateXIndex = (xIndex % this.tileWidthInVertices);

    //     TileCoordinate tileCoordinate = new TileCoordinate(tileZIndex, tileXIndex, coordinateZIndex, coordinateXIndex);
    //     return tileCoordinate;
    //  }

     public void AddTileData(TileData tileData, int tileZIndex, int tileXIndex)
     {
         if(tileZIndex < 0 && tileXIndex < 0)
         {
            negativeIJTilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = tileData;
         }
         else if(tileZIndex < 0 && tileXIndex >= 0)
         {
             negativeITilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = tileData;
         }
         else if(tileZIndex >= 0 && tileXIndex < 0)
         {
             negativeJTilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = tileData;
         }
         else
         {
            positiveTilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)] = tileData;
         }
     }

     //method to find chunk from correct data structure depending on coordinates
     public TileData FindChunk(int tileZIndex, int tileXIndex)
     {
         TileData tileData = null;

         if(tileZIndex < 0 && tileXIndex < 0)
         {
            tileData = negativeIJTilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)];
         }
         else if(tileZIndex < 0 && tileXIndex >= 0)
         {
             tileData = negativeITilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)];
         }
         else if(tileZIndex >= 0 && tileXIndex < 0)
         {
             tileData = negativeJTilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)];
         }
         else
         {
            tileData = positiveTilesData[System.Math.Abs(tileZIndex), System.Math.Abs(tileXIndex)];
         }
         return tileData;
     }
}

//class to represent a coordinate in the Tile coordinate system
// public class TileCoordinate
// {
//     public int tileZIndex;
//     public int tileXIndex;
//     public int coordinateZIndex;
//     public int coordinateXIndex;

//     public TileCoordinate(int tileZIndex, int tileXIndex, int coordinateZIndex, int coordinateXIndex)
//     {
//         this.tileZIndex = tileZIndex;
//         this.tileXIndex = tileXIndex;
//         this.coordinateZIndex = coordinateZIndex;
//         this.coordinateXIndex = coordinateXIndex;
//     }
// }