using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollowMouse : MonoBehaviour
{
    //LateUpdate happends after Update
    void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
