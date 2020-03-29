using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeData
{
    public float[] position;

    public TreeData(TallTree tree)
    {
        position = new float[3];
        position[0] = tree.transform.position.x;
        position[1] = tree.transform.position.y;
        position[2] = tree.transform.position.z;
    }
}
