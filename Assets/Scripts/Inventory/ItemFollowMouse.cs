using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollowMouse : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
