using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StumpData
{
    public float[] position;
    public string title = "OakTreeStump";

    //public int health = 10;

    public bool canBeUnder = false;
    public bool canBeOver = false;

    public string GetTitle()
    {
        return this.title;
    }

    public StumpData(TreeStump stump)
    {
        //position including offset
        position = new float[3];
        position[0] = stump.transform.position.x;
        position[1] = stump.transform.position.y;
        position[2] = stump.transform.position.z;

        this.title = stump.gameObject.name;
    }
}