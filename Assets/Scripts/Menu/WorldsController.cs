using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class WorldsController : MonoBehaviour
{
    public GameObject worldNameInputText;
    public GameObject worldDetailsList;
    private PassData passData;

    private string currentChosenWorld;

    void Awake()
    {
        passData = GameObject.Find("PassInfo").GetComponent<PassData>();

        string[] worldDirectories = loadWorldData();
        foreach (var dir in worldDirectories)
        {
            //generate worldData UI element and add to content list
            GenerateWorldDataUI(dir);
        }
    }

    /*
    Setter for currentChosenWorld (world currently selected to play/edit/delete)
    */
    public void SetCurrentSelectedWorld(GameObject world)
    {
        this.currentChosenWorld = GetWorldNameFromPath(world.GetComponent<Text>().text.ToString());
        Debug.Log(this.currentChosenWorld);
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

    //method to generate WorldData prefab and add to content list
    public void GenerateWorldDataUI(string path)
    {
        GameObject worldDetailsUI = Instantiate(Resources.Load<GameObject>("UIElements/WorldDetails")) as GameObject;

        //pass setCurrentSelectedWorld() as a delegate on the onClick event of the button
        //worldDetailsUI.onClick.AddListener(delegate{this.SetCurrentSelectedWorld({GetWorldNameFromPath(path)});});

        if(worldDetailsUI != null)
        {
            //set (first child) WorldName Text to worlName from path
            worldDetailsUI.transform.GetChild(0).GetComponent<Text>().text = GetWorldNameFromPath(path);

            //add new worldDetails as child of content list
            worldDetailsUI.transform.SetParent(this.worldDetailsList.transform);

        }else
        {
            Debug.Log("worldDetials prefab is NULL");
        }
    }
}
