using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGen : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile dirtTile;
    public Tile waterTile;
    private GameObject player;

    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;

    [SerializeField]
    private float mapScale = 5;

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
        int tileDepth = 64;
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

                //render tiles depending on height
                if(height < 0.5)
                {
                    //low
                    tilemap.SetTile(new Vector3Int(xIndex, zIndex, 0), dirtTile);
                }else if(height >= 0.5)
                {
                    //high
                    tilemap.SetTile(new Vector3Int(xIndex, zIndex, 0), waterTile);
                }
            }
        }
    }

    void Update()
    {
        // tilemap.BoxFill(tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()), dirtTile, 
        // tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).x - 4,
        // tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).y - 4,
        // tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).x + 4,
        // tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).y + 4);
    }
}
