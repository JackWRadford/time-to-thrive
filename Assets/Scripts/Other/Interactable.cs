using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interactable : MonoBehaviour
{
    public virtual void Interact(GameObject go)
    {
        //Debug.Log("Interacting with: " + transform.name);
    }

    public virtual void NDInteract(GameObject go)
    {
        //Debug.Log("NDInteracting with: " + transform.name);
    }
}
