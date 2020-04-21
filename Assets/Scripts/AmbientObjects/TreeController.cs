using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeController : MonoBehaviour
{
    // public GameObject tree;
    // public float[] position;
    // //public List<float[]> treePositions = new List<float[]>();
    // void Awake()
    // {
    //     //GameEvents.SaveInitiated += Save;
    //     //GameEvents.LoadInitiated += Load;
    // }

    // void Start()
    // {
    //     //Load();
    // }

    // public void RemoveTree(float[] treeToRemove)
    // {
    //     //FindAndRemove(treeToRemove);
    //     treePositions.RemoveAll(a => a.SequenceEqual(treeToRemove));
    // }

    // public bool CheckSpaceFree(float[] pos)
    // {
    //     if(treePositions != null)
    //         {
    //             //check tree not already in that position
    //             foreach (var arr in treePositions)
    //             {
    //                 if(arr.SequenceEqual(pos))
    //                 {
    //                     //tree already exists in that location
    //                     return false;
    //                 }
    //             }
    //         }
    //     return true;
    // }

    // public void GenerateTrees(int numberOfTrees)
    // {
    //     for(int i = 0; i < numberOfTrees; i++){
    //         float x = Random.Range(-10, 11) + 0.5f;
    //         float y = Random.Range(-10, 11) + 0.5f;
    //         float[] randPos = {x,y,0};
    //         if(CheckSpaceFree(randPos))
    //         {
    //             GameObject t = Instantiate(tree, new Vector3(x,y,0), Quaternion.identity);
    //             //get tree transform as List of three floats
    //             PopulateTreePositions(t);
    //         }
    //         else
    //         {
    //             Debug.Log("Stopped duplicate");
    //             i--;
    //         }
    //     }
    // }

    // public void SpawnSavedTrees(List<float[]> treesToSpawn)
    // {
    //     foreach (var t in treesToSpawn)
    //     {
    //         GameObject loadedTree = Instantiate(tree, new Vector3(t[0], t[1], 0), Quaternion.identity);
    //         PopulateTreePositions(loadedTree);
    //     }
    // }

    // public void PopulateTreePositions(GameObject t)
    // {
    //     TallTree tts = t.GetComponent<TallTree>();
    //     TreeData td = new TreeData(tts);
    //     treePositions.Add(td.position);
    // }

    // void Save()
    // {
    //     SaveSystem.Save<List<float[]>>(treePositions, "Trees");
    //     Debug.Log("saved trees");
    // }

    // void Load()
    // {
    //     Debug.Log("try to load trees");
    //     if(SaveSystem.SaveExists("Trees"))
    //     {
    //         Debug.Log("Save Exists");
    //         SpawnSavedTrees(SaveSystem.Load<List<float[]>>("Trees"));
    //     }
    //     else
    //     {
    //         Debug.Log("Generate random trees");
    //         GenerateTrees(10);
    //     }
    // }

}
