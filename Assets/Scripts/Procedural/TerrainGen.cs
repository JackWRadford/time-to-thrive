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

    [SerializeField]
    private TerrainType[] terrainTypes;

    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        GenerateTile();
    }

    void GenerateTile()
    {
        int tileDepth = 32;
        int tileWidth = tileDepth;

        float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, this.mapScale);

        RenderTiles(heightMap);
    }

    void RenderTiles(float[,] heightMap)
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
                tilemap.SetTile(new Vector3Int(xIndex, zIndex, 0), GetTerrainTypeForHeight(height).tile);
                
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