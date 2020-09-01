using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockData
{
    public float[] position;
    public string title = "Rock";
    public int stage = 0;
    public int health = 0;

    public bool canBeUnder = false;
    public bool canBeOver = false;

    public string GetTitle()
    {
        return this.title;
    }

    public RockData(RockController r)
    {
        //position including offset
        position = new float[3];
        position[0] = r.transform.position.x;
        position[1] = r.transform.position.y;
        position[2] = r.transform.position.z;

        //set title to name of tree being saved
        this.title = r.gameObject.name;

        this.stage = r.stage;

        this.health = r.GetHealth();
    }
}
