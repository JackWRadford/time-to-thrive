using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    //static bool created = false;
    private static DoNotDestroy doNotDestroyInstance;

    void Awake()
    {
        //keep between scenes
        DontDestroyOnLoad(this);
        if(doNotDestroyInstance == null)
        {
            doNotDestroyInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
