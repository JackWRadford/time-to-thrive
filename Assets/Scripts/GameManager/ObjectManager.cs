using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    Dictionary<List<float>, string> objectsString = new Dictionary<List<float>, string>();
    //Dictionary<List<float>, GameObject> objectsGO = new Dictionary<List<float>, GameObject>();

    void Awake()
    {
        GameEvents.SaveInitiated += Save;
    }

    void Start()
    {
        Load();
    }

    public void AddObject(float x, float y, string title, GameObject go)
    {
        List<float> pos = new List<float>{x,y};
        objectsString.Add(pos, title);
        //objectsGO.Add(pos, go);
    }

    public void RemoveObject(float x, float y)
    {
        
    }

    public bool IsSpaceFree(float x, float y)
    {
        List<float> pos = new List<float>{x,y};
        foreach (var objPos in objectsString.Keys)
        {
            if(objPos.SequenceEqual(pos))
            {
                Debug.Log("space filled");
                return false;
            }
        }
        Debug.Log("space free");
        return true;
    }

    public bool IsFenceAdjacent(float x, float y)
    {
        
        return false;
    }

    public void SpawnSavedObjects(Dictionary<List<float>, string> objects)
    {
        foreach (var obj in objects)
        {
            GameObject go = Resources.Load<GameObject>("Placeable/" + obj.Value);
            Instantiate(go, new Vector3(obj.Key[0], obj.Key[1], 0), Quaternion.identity);
            //populate list of objects
            objectsString.Add(obj.Key, obj.Value);
        }
    }

    void Save()
    {
        SaveSystem.Save<Dictionary<List<float>, string>>(objectsString, "Objects");
        Debug.Log("saved objects");
    }

    void Load()
    {
        Debug.Log("try to objects");
        if(SaveSystem.SaveExists("Objects"))
        {
            Debug.Log("Save Exists");
            SpawnSavedObjects(SaveSystem.Load<Dictionary<List<float>, string>>("Objects"));
        }
    }
}
