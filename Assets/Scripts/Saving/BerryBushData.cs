using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BerryBushData
{
    public float[] position;
    public string title = "BerryBush";
    public int stage = 0;

    public bool canBeUnder = false;
    public bool canBeOver = false;

    public string GetTitle()
    {
        return this.title;
    }

    public BerryBushData(BerryBushController bbc)
    {
        //position including offset
        position = new float[3];
        position[0] = bbc.transform.position.x;
        position[1] = bbc.transform.position.y;
        position[2] = bbc.transform.position.z;

        //set title to name of tree being saved
        this.title = bbc.gameObject.name;

        this.stage = bbc.stage;
    }
}
