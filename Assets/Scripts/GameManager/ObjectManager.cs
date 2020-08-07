using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    public GenerateWorld generateWorld;
    public TerrainGen terrainGen;

    private bool constructionIsSeeThrough = false;
    public float timerLength = 5.0f;
    private float timer = 0.0f;
    private bool antiAllowed = false;
    private float currentX = 0f;
    private float currentY = 0f;

    //private Dictionary<List<float>, string> objectsString = new Dictionary<List<float>, string>();
    //private Dictionary<List<float>, dynamic> objectsGO = new Dictionary<List<float>, dynamic>();

    void Awake()
    {
        generateWorld = GetComponent<GenerateWorld>();
        terrainGen = GetComponent<TerrainGen>();

        this.timer = timerLength;
        //un-comment (whole world)
        //GameEvents.SaveInitiated += Save;
    }

    void Start()
    {
        //un-comment (whole world)
        //Load();
    }

    void Update()
    {
        if(this.constructionIsSeeThrough)
        {
            timer -= Time.deltaTime;
        }
        
        if(timer <= 0.0f)
        {
            TimerDone();
        }
    }

    // public Dictionary<List<float>, dynamic> GetObjectGOs()
    // {
    //     return this.objectsGO;
    // }

    // // public Dictionary<List<float>, string> GetObjects()
    // // {
    // //     return this.objectsString;
    // // }

    // public Dictionary<List<float>, dynamic> GetObjects()
    // {
    //     return this.objectsGO;
    // }

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
        x += 0.5f;
        y += 0.5f;
        //get chunk that coordinates are in
        int offsetX = terrainGen.GetChunkCoordsFromWorld(x, y)[0];
        int offsetY = terrainGen.GetChunkCoordsFromWorld(x, y)[1];

        //find chunk from levelData
        tileData = terrainGen.GetLevelData().FindChunk(offsetX, offsetY);

        return tileData;
    }

    public void AddObjectData(float x, float y, string title, dynamic data)
    {
        //List<float> pos = new List<float>{x,y};

        //convert to save data for that object (?already done)

        //add object data and position to matrix to be saved
        //objectsGO.Add(pos, data);

        //objectsString.Add(pos, title);

        //call relevant chunk addObject method
        FindChunkFromCoords(x, y).AddObjectData(x, y, title, data);
    }

    public void AddObjectGO(float x, float y, string title, GameObject go)
    {
        FindChunkFromCoords(x, y).AddObjectGO(x, y, title, go);
    }

    public bool IsAboveAnother(float x, float y, GameObject obj)
    {
        return FindChunkFromCoords(x, y).IsAboveAnother(x, y, obj);
    }

    public bool IsGOAboveAnother(float x, float y, GameObject obj)
    {
        return FindChunkFromCoords(x, y).IsGOAboveAnother(x, y, obj);
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

    public void RemoveObjectGO(float x, float y)
    {
        //call relevant chunk removeObject method
        FindChunkFromCoords(x, y).RemoveObjectGO(x, y);
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

    public bool IsSpaceFree(float x, float y, GameObject obj)
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
        return FindChunkFromCoords(x, y).IsSpaceFree(x, y, obj);
    }

    //check if single space is free
    public bool IsGOSpaceFree(float x, float y, GameObject obj)
    {
        return FindChunkFromCoords(x, y).IsGOSpaceFree(x, y, obj);
    }

    //check if multiple spaces are free (call findChunkFromCoords with adjusted coords too)
    public bool IsGOSpaceFreeMultiple(float x, float y, List<Vector3> listOfPositions, GameObject obj)
    {
        bool allAreFree = true;

        //check if "main" space or any others specified are not free
        if(!IsGOSpaceFree(x, y, obj))
        {
            return false;
        }
        foreach (var pos in listOfPositions)
        {
            if(!IsGOSpaceFree(x + pos.x, y + pos.y, obj))
            {
                allAreFree = false;
                break;
            }
        }

        return allAreFree;
    }

    //method to make ExternalContruction objects SeeThrough (check current chunk and surrounding chunks)
    public void SeeBehindExternalContruction(float x, float y)
    {
        this.timer = this.timerLength;
        if(!this.constructionIsSeeThrough)
        {
            this.currentX = x;
            this.currentY = y;

            FindChunkFromCoords(x - TerrainGen.chunkSize, y + TerrainGen.chunkSize).MakeConstructionSeeThrough();
            FindChunkFromCoords(x, y + TerrainGen.chunkSize).MakeConstructionSeeThrough();
            FindChunkFromCoords(x + TerrainGen.chunkSize, y + TerrainGen.chunkSize).MakeConstructionSeeThrough();
            FindChunkFromCoords(x - TerrainGen.chunkSize, y).MakeConstructionSeeThrough();
            FindChunkFromCoords(x, y).MakeConstructionSeeThrough();
            FindChunkFromCoords(x + TerrainGen.chunkSize, y).MakeConstructionSeeThrough();
            FindChunkFromCoords(x - TerrainGen.chunkSize, y - TerrainGen.chunkSize).MakeConstructionSeeThrough();
            FindChunkFromCoords(x, y - TerrainGen.chunkSize).MakeConstructionSeeThrough();
            FindChunkFromCoords(x + TerrainGen.chunkSize, y - TerrainGen.chunkSize).MakeConstructionSeeThrough();

            this.constructionIsSeeThrough = true;
        } 
    }

    //method to make ExternalContruction objects AntiSeeThrough (check current chunk and surrounding chunks)
    public void AntiSeeBehindExternalContruction(float x, float y)
    {
        if((this.constructionIsSeeThrough)&&(this.antiAllowed))
        {
            this.currentX = x;
            this.currentY = y;

            FindChunkFromCoords(x - TerrainGen.chunkSize, y + TerrainGen.chunkSize).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x, y + TerrainGen.chunkSize).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x + TerrainGen.chunkSize, y + TerrainGen.chunkSize).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x - TerrainGen.chunkSize, y).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x, y).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x + TerrainGen.chunkSize, y).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x - TerrainGen.chunkSize, y - TerrainGen.chunkSize).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x, y - TerrainGen.chunkSize).AntiMakeConstructionSeeThrough();
            FindChunkFromCoords(x + TerrainGen.chunkSize, y - TerrainGen.chunkSize).AntiMakeConstructionSeeThrough();

            this.constructionIsSeeThrough = false;
            this.antiAllowed = false;
            this.timer = this.timerLength;
        }
    }

    public void TimerDone()
    {
        //Debug.Log("timer done");
        AntiSeeBehindExternalContruction(this.currentX, this.currentY);
        this.antiAllowed = true;
    }


    //method to check if space is free in position and surrounding positions (useful for multiTile objects?(size parameter))
    // public bool IsGOSpaceFreeMultiChunk(float x, float y, GameObject obj)
    // {
    //     //if space is free in a surrounding possition (possibly in a different chunk)
    //     bool ans = false;
    //     if((FindChunkFromCoords(x, y).IsGOSpaceFree(x, y, obj))||(FindChunkFromCoords(x, y).IsGOSpaceFree(x, y, obj))
    //     ||(FindChunkFromCoords(x, y).IsGOSpaceFree(x, y, obj))||(FindChunkFromCoords(x, y).IsGOSpaceFree(x, y, obj)))
    //     {
    //         ans = true;
    //     }

    //     return ans;
    // }

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

    // public void SpawnSavedObjects(Dictionary<List<float>, dynamic> objects)
    // {
    //     foreach (var obj in objects)
    //     {
    //         GameObject go = Resources.Load<GameObject>("Placeable/" + obj.Value.GetTitle());
    //         GameObject goi = Instantiate(go, new Vector3(obj.Key[0], obj.Key[1], 0), Quaternion.identity);
    //         goi.GetComponent<ILoadState>().LoadState(obj.Value);
    //         //load state from save onto instantiated gameObject
    //         // ILoadState script = goi.GetComponent<ILoadState>();
    //         // //print(obj.Value.health);
    //         // script.LoadState(obj.Value);

            

    //         //populate list of objects
    //         objectsGO.Add(obj.Key, obj.Value);
    //     }
    // }

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

    // void Save()
    // {
    //     SaveSystem.Save<Dictionary<List<float>, dynamic>>(objectsGO, "Objects");
    //     //SaveSystem.Save<Dictionary<List<float>, dynamic>>(objectsGO, "Objects");
    //     Debug.Log("saved objects");
    // }

    // void Load()
    // {
    //     //Debug.Log("try to objects");
    //     if(SaveSystem.SaveExists("Objects"))
    //     {
    //         Debug.Log("Objects Save Exists");
    //         SpawnSavedObjects(SaveSystem.Load<Dictionary<List<float>, dynamic>>("Objects"));
    //         //SpawnSavedObjects(SaveSystem.Load<Dictionary<List<float>, dynamic>>("Objects"));
    //     }
    //     else
    //     {
    //         //generate world
    //         Debug.Log("No Objects Save");
    //         //generateWorld.Generate();
    //     }
    // }
}
