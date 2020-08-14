using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class WorldsController : MonoBehaviour
{
    public GameObject worldNameInputText;
    private PassData passData;

    private string worldName;

    void Awake()
    {
        passData = GameObject.Find("PassInfo").GetComponent<PassData>();

        string[] worldDirectories = loadWorldData();
        foreach (var dir in worldDirectories)
        {
            Debug.Log(GetWorldNameFromPath(dir));
        }
    }
    
    //method to add new world
    public void AddNewWorld()
    {
        if(this.worldNameInputText.GetComponent<InputField>().text != null)
        {
            passData.SetWorldName(this.worldNameInputText.GetComponent<InputField>().text);
            Debug.Log(this.worldNameInputText.GetComponent<InputField>().text.ToString());
        }
    }

    //method to get worldName (data) from game save file
    public string[] loadWorldData()
    {
        string path = Application.persistentDataPath + "/saves/";

        //get save data and load into WorldDetails Prefabs
        return Directory.GetDirectories(path);
    }

    //method to just get worldName from whole path
    public string GetWorldNameFromPath(string path)
    {
        return Regex.Match(path, "[^/]+$").ToString();
    }

    //method to generate WorldData prefab
    public void GenerateWorldDataUI()
    {
        
    }
}
