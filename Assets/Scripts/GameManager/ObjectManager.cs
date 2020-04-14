using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    Dictionary<List<float>, string> objectsString = new Dictionary<List<float>, string>();
    Dictionary<List<float>, GameObject> objectsGO = new Dictionary<List<float>, GameObject>();

    public void AddObject(float x, float y, string title, GameObject go)
    {
        List<float> pos = new List<float>{x,y};
        objectsString.Add(pos, title);
        objectsGO.Add(pos, go);
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
}
