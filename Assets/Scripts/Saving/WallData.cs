using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallData
{
    public float[] position;
    public string title = "Wall_0";
    public bool canBeUnder = true;
    public bool canBeOver = true;

    public string GetTitle()
    {
        return this.title;
    }

    public WallData(WallController wall)
    {
        //position including offset
        position = new float[3];
        position[0] = wall.transform.position.x;
        position[1] = wall.transform.position.y;
        position[2] = wall.transform.position.z;
        
        this.title = wall.gameObject.name;
    }
}
