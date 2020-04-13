using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    Dictionary<List<float>, string> objects = new Dictionary<List<float>, string>();

    public void AddObject(float x, float y, string title)
    {
        List<float> pos = new List<float>{x,y};
        objects.Add(pos, title);
    }

    public void RemoveObject()
    {

    }

    public bool IsSpaceFree(float x, float y)
    {
        List<float> pos = new List<float>{x,y};
        foreach (var objPos in objects.Keys)
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
}
