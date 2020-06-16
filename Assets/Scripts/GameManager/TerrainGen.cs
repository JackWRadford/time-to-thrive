using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGen : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile dirtTile;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        print("generate init terrain");
        //tilemap.SetTile(player.GetPos)
        //tilemap.BoxFill(new Vector3Int(0,0,0), dirtTile, 0, 0, 16, 16);
    }

    void Update()
    {
        //tilemap.BoxFill(toV3Int(player.GetComponent<PlayerController>().GetPosition()), dirtTile, 0, 0, 8, 8);
        //tilemap.SetTile(toV3Int(player.GetComponent<PlayerController>().GetPosition()), dirtTile);
        //tilemap.SetTile(tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()), dirtTile);
        tilemap.BoxFill(tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()), dirtTile, 
        tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).x - 4,
        tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).y - 4,
        tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).x + 4,
        tilemap.WorldToCell(player.GetComponent<PlayerController>().GetPosition()).y + 4);
    }

    // Vector3Int toV3Int(Vector3 v3)
    // {
    //     return new Vector3Int((int)v3.x, (int)v3.y, (int)v3.z);
    // }
}
