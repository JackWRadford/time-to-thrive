using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GameObject tree;
    public float[] position;
    public List<float[]> treePositions = new List<float[]>();
    void Start()
    {
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;
    }

    public void GenerateTrees()
    {
        for(int i = 0; i < 18; i++){
            GameObject t = Instantiate(tree, new Vector3(Random.Range(-10, 11) + 0.5f,Random.Range(-10, 11)+0.5f,0), Quaternion.identity);
            //get tree transform as List of three floats
            PopulateTreePositions(t);
        }
    }

    public void SpawnSavedTrees(List<float[]> treesToSpawn)
    {
        foreach (var t in treesToSpawn)
        {
            GameObject loadedTree = Instantiate(tree, new Vector3(t[0], t[1], 0), Quaternion.identity);
            position = new float[3];
            PopulateTreePositions(loadedTree);
        }
    }

    public void PopulateTreePositions(GameObject t)
    {
        position = new float[3];
        position[0] = t.transform.position.x;
        position[1] = t.transform.position.y;
        position[2] = t.transform.position.z;
        treePositions.Add(position);
    }

    void Save()
    {
        SaveSystem.Save<List<float[]>>(treePositions, "Trees");
    }

    void Load()
    {
        if(SaveSystem.SaveExists("Trees"))
        {
            Debug.Log("Save Exists");
            SpawnSavedTrees(SaveSystem.Load<List<float[]>>("Trees"));
        }
        else
        {
            GenerateTrees();
        }
    }

}
