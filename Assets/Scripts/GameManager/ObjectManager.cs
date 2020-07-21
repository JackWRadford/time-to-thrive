using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    public GenerateWorld generateWorld;
    public TerrainGen terrainGen;

    //private Dictionary<List<float>, string> objectsString = new Dictionary<List<float>, string>();
    private Dictionary<List<float>, dynamic> objectsGO = new Dictionary<List<float>, dynamic>();

    void Awake()
    {
        generateWorld = GetComponent<GenerateWorld>();
        terrainGen = GetComponent<TerrainGen>();

        //un-comment
        //GameEvents.SaveInitiated += Save;
    }

    void Start()
    {
        //un-comment
        //Load();
    }

    public Dictionary<List<float>, dynamic> GetObjectGOs()
    {
        return this.objectsGO;
    }

    // public Dictionary<List<float>, string> GetObjects()
    // {
    //     return this.objectsString;
    // }

    public Dictionary<List<float>, dynamic> GetObjects()
    {
        return this.objectsGO;
    }

    // public void AddObject(float x, float y, string title, GameObject data)
    // {
    //     List<float> pos = new List<float>{x,y};

    //     //convert to save data for that object
    //     objectsGO.Add(pos, data);

    //     //objectsString.Add(pos, title);
        
    // }

    //method to find chunk from world coordinates
    public TileData FindChunkFromCoords(float x, float y)
    {
        TileData tileData = null;
        //get chunk that coordinates are in
        int offsetX = terrainGen.GetChunkCoordsFromWorld(x, y)[0];
        int offsetY = terrainGen.GetChunkCoordsFromWorld(x, y)[1];

        //find chunk from levelData
        tileData = terrainGen.GetLevelData().FindChunk(offsetX, offsetY);

        return tileData;

    }

    public void AddObject(float x, float y, string title, dynamic data)
    {
        //List<float> pos = new List<float>{x,y};

        //convert to save data for that object (?already done)

        //add object data and position to matrix to be saved
        //objectsGO.Add(pos, data);

        //objectsString.Add(pos, title);

        //call relevant chunk addObject method
        FindChunkFromCoords(x, y).AddObject(x, y, title, data);
    }

    // public void UpdateObject(float x, float y, string title, dynamic data)
    // {
    //     List<float> pos = new List<float>{x,y};

    //     objectsGO.Add(pos, data);

    //     //objectsString.Add(pos, title);
        
    // }

    // public void RemoveObject(float x, float y)
    // {
    //     List<float> pos = new List<float>{x,y};
    //     foreach (var objPos in objectsString.Keys)
    //     {
    //         if(objPos.SequenceEqual(pos))
    //         {
    //             Debug.Log("remove from dictionary:" + objectsString[objPos]);
    //             objectsString.Remove(objPos);
    //             return;
    //         }
    //     }
    // }

    public void RemoveObject(float x, float y)
    {
        // List<float> pos = new List<float>{x,y};
        // foreach (var objPos in objectsGO.Keys)
        // {
        //     if(objPos.SequenceEqual(pos))
        //     {
        //         Debug.Log("remove from dictionary:" + objectsGO[objPos]);
        //         objectsGO.Remove(objPos);
        //         return;
        //     }
        // }

        //call relevant chunk removeObject method
        FindChunkFromCoords(x, y).RemoveObject(x, y);
    }

    // public bool IsSpaceFree(float x, float y)
    // {
    //     List<float> pos = new List<float>{x,y};
    //     foreach (var objPos in objectsString.Keys)
    //     {
    //         if(objPos.SequenceEqual(pos))
    //         {
    //             Debug.Log("space filled");
    //             return false;
    //         }
    //     }
    //     Debug.Log("space free");
    //     return true;
    // }

    public bool IsSpaceFree(float x, float y)
    {
        // List<float> pos = new List<float>{x,y};
        // foreach (var objPos in objectsGO.Keys)
        // {
        //     if(objPos.SequenceEqual(pos))
        //     {
        //         //Debug.Log("space filled");
        //         return false;
        //     }
        // }
        // //Debug.Log("space free");
        // return true;

        //call relevant chunk isSpaceFree method
        return FindChunkFromCoords(x, y).IsSpaceFree(x, y);
    }

    // public void SpawnSavedObjects(Dictionary<List<float>, string> objects)
    // {
    //     foreach (var obj in objects)
    //     {
    //         GameObject go = Resources.Load<GameObject>("Placeable/" + obj.Value);
    //         Instantiate(go, new Vector3(obj.Key[0], obj.Key[1], 0), Quaternion.identity);
    //         //populate list of objects
    //         objectsString.Add(obj.Key, obj.Value);
    //     }
    // }

    public void SpawnSavedObjects(Dictionary<List<float>, dynamic> objects)
    {
        foreach (var obj in objects)
        {
            GameObject go = Resources.Load<GameObject>("Placeable/" + obj.Value.GetTitle());
            GameObject goi = Instantiate(go, new Vector3(obj.Key[0], obj.Key[1], 0), Quaternion.identity);
            goi.GetComponent<ILoadState>().LoadState(obj.Value);
            //load state from save onto instantiated gameObject
            // ILoadState script = goi.GetComponent<ILoadState>();
            // //print(obj.Value.health);
            // script.LoadState(obj.Value);

            

            //populate list of objects
            objectsGO.Add(obj.Key, obj.Value);
        }
    }

    // public void SpawnSavedObjects(Dictionary<List<float>, dynamic> objects)
    // {
    //     foreach (var obj in objects)
    //     {
    //         GameObject go = Resources.Load<GameObject>("Placeable/" + obj.Value.GetTitle());
    //         Instantiate(go, new Vector3(obj.Key[0], obj.Key[1], 0), Quaternion.identity);
    //         //populate list of objects
    //         // objectsString.Add(obj.Key, obj.Value);
    //         objectsGO.Add(obj.Key, obj.Value);
    //     }
    // }

    // void Save()
    // {
    //     SaveSystem.Save<Dictionary<List<float>, string>>(objectsString, "Objects");
    //     //SaveSystem.Save<Dictionary<List<float>, dynamic>>(objectsGO, "Objects");
    //     Debug.Log("saved objects");
    // }

    void Save()
    {
        SaveSystem.Save<Dictionary<List<float>, dynamic>>(objectsGO, "Objects");
        //SaveSystem.Save<Dictionary<List<float>, dynamic>>(objectsGO, "Objects");
        Debug.Log("saved objects");
    }

    void Load()
    {
        //Debug.Log("try to objects");
        if(SaveSystem.SaveExists("Objects"))
        {
            Debug.Log("Objects Save Exists");
            SpawnSavedObjects(SaveSystem.Load<Dictionary<List<float>, dynamic>>("Objects"));
            //SpawnSavedObjects(SaveSystem.Load<Dictionary<List<float>, dynamic>>("Objects"));
        }
        else
        {
            //generate world
            Debug.Log("No Objects Save");
            //generateWorld.Generate();
        }
    }
}
