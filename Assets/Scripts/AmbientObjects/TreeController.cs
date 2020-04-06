using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeController : MonoBehaviour
{
    public GameObject tree;
    public float[] position;
    public List<float[]> treePositions = new List<float[]>();
    void Awake()
    {
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;
    }

    public void RemoveTree(float[] treeToRemove)
    {
        //FindAndRemove(treeToRemove);
        treePositions.RemoveAll(a => a.SequenceEqual(treeToRemove));
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
            PopulateTreePositions(loadedTree);
        }
    }

    public void PopulateTreePositions(GameObject t)
    {
        TallTree tts = t.GetComponent<TallTree>();
        TreeData td = new TreeData(tts);
        treePositions.Add(td.position);
    }

    void Save()
    {
        SaveSystem.Save<List<float[]>>(treePositions, "Trees");
        Debug.Log("saved trees");
    }

    void Load()
    {
        Debug.Log("try to load trees");
        if(SaveSystem.SaveExists("Trees"))
        {
            Debug.Log("Save Exists");
            SpawnSavedTrees(SaveSystem.Load<List<float[]>>("Trees"));
        }
        else
        {
            Debug.Log("Generate random trees");
            GenerateTrees();
        }
    }

}
