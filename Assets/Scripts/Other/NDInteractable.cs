using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NDInteractable : MonoBehaviour
{
    public virtual void NDInteract(GameObject go)
    {
        //Debug.Log("NDInteracting with: " + transform.name);
    }
}