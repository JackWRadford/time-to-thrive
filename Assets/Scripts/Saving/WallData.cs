using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallData
{
    public float[] position;
    public string title = "Wall";

    public string GetTitle()
    {
        return this.title;
    }

    public WallData(WallController wall)
    {
        position = new float[3];
        position[0] = wall.transform.position.x;
        position[1] = wall.transform.position.y;
        position[2] = wall.transform.position.z;
    }
}
