using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TerrainGen : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile dirtTile;
    public Tile waterTile;
    public Tile grassTile;
    private GameObject player;

    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;

    [SerializeField]
    private float mapScale = 0;

    //size of tiles made with perlin noise
    private int chunkSize = 32;

    [SerializeField]
    private TerrainType[] terrainTypes = null;

    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
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

        float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ);

        RenderTiles(heightMap, (int)offsetX, (int)offsetZ);
    }

    void RenderTiles(float[,] heightMap, int oX, int oZ)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];

                //get tarrain type for given height and add it to the tilemap
                tilemap.SetTile(new Vector3Int(xIndex + oX, zIndex + oZ, 0), GetTerrainTypeForHeight(height).tile);
                
                //render tiles depending on height
                // if(height <= 0.4)
                // {
                //     //water
                //     tilemap.SetTile(new Vector3Int(xIndex, zIndex, 0), terrainTypes[0].tile);
                // }else if(height <= 0.7)
                // {
                //     //grass
                //     tilemap.SetTile(new Vector3Int(xIndex, zIndex, 0), terrainTypes[1].tile);
                // }else if(height <= 1)
                // {
                //     //mountain
                //     tilemap.SetTile(new Vector3Int(xIndex, zIndex, 0), terrainTypes[2].tile);
                // }
            }
        }
    }

    //method to return terrainType depending on the height input
    TerrainType GetTerrainTypeForHeight(float height)
    {
        foreach(TerrainType terrainType in terrainTypes)
        {
            if(height < terrainType.height)
            {
                //correct height
                return terrainType;
            }
        }
        //if no terrainTypes apply return the last (highest) one (sometimes perlinNoise return > 1 (or < 0))
        return terrainTypes[terrainTypes.Length -1];
    }
}

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Tile tile;
}