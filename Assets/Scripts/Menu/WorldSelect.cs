using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelect : MonoBehaviour
{
    WorldsController worldsController;

    void Awake()
    {
        worldsController = GameObject.Find("Canvas").GetComponent<WorldsController>();
    }

    /*
    method to set selectedWorld in worldsController to be played/editied/deleted
    */
    public void SetSelectedWorld(GameObject world)
    {
        worldsController.SetCurrentSelectedWorld(world);
    }
}
