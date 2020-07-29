using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FenceData
{
    public float[] position;

    public string title = "Fence";

    public string GetTitle()
    {
        return this.title;
    }

    public FenceData(FenceController fence)
    {
        //position including offset
        position = new float[3];
        position[0] = fence.transform.position.x;
        position[1] = fence.transform.position.y;
        position[2] = fence.transform.position.z;
    }
}
