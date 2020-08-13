using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassData : MonoBehaviour
{
    private static GameObject instance;
    private string worldName;

    void Awake()
    {
        //instance = GameObject.Find("PassInfo");

        //check not creating duplicate
        if(instance == null)
        {
            DontDestroyOnLoad(this.transform.gameObject);
            instance = this.transform.gameObject;
        }
        else{
            Destroy(gameObject);
        }
    }

    public void SetWorldName(string name)
    {
        this.worldName = name;
    }

    public string GetWorldName()
    {
        return this.worldName;
    }
}
