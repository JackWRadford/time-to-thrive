using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    [SerializeField]
    private NoiseMapGeneration noiseMapGeneration;
    
    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private float neightbourRadius;

    [SerializeField]
    private GameObject treePrefab;

    void Awake()
    {
        noiseMapGeneration = this.GetComponent<NoiseMapGeneration>();
    }

    //method to generate trees for given chunk
    public void GenerateTrees(TileData tileData)
    {

    }
}
