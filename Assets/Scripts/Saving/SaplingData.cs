using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaplingData
{
    public float[] position;
    public string title = "OakTreeSapling";

    public string GetTitle()
    {
        return this.title;
    }

    public SaplingData(TreeSapling s)
    {
        position = new float[3];
        position[0] = s.transform.position.x;
        position[1] = s.transform.position.y;
        position[2] = s.transform.position.z;

        this.title = s.gameObject.name;
    }
}
