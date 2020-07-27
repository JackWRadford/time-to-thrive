using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementOffset : MonoBehaviour
{
    //offset is given in pixels (16pixels to 1 world coordinate)
    [SerializeField]
    private float offsetX = 0;
    [SerializeField]
    private float offsetY = 0;

    private int pixlesPerUnit = 16;

    public float GetOffsetX()
    {
        return this.offsetX / pixlesPerUnit;
    }

    public float GetOffsetY()
    {
        return this.offsetY / pixlesPerUnit;
    }
}
