using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDetails : MonoBehaviour
{
    public bool canBeOver = false;
    public bool canBeUnder = false;

    //if being placed save to objects list (don't if being loaded)
    public bool placeing = false;

    //height in world coordinates
    public float height = 0;

    public void SetPlaceing(bool tof)
    {
        this.placeing = tof;
    }

    public void DestroyGO()
    {
        Destroy(this.gameObject);
    }
}
