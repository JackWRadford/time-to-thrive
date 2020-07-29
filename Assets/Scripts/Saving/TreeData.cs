using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeData
{
    public float[] position;
    public string title = "Tree";

    //public int health = 10;

    public string GetTitle()
    {
        return this.title;
    }

    public TreeData(TallTree tree)
    {
        //position including offset
        position = new float[3];
        position[0] = tree.transform.position.x;
        position[1] = tree.transform.position.y;
        position[2] = tree.transform.position.z;

        //this.health = tree.GetHealth();
        //set title to name of tree being saved (oak, savanna, spruce)
        this.title = tree.gameObject.name;
    }
}
