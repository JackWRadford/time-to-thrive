using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

public class WorldsController : MonoBehaviour
{
    public GameObject worldNameInputText;
    public GameObject worldDetailsList;
    public GameObject actualFilePathText;
    private PassData passData;

    private string currentChosenWorld;

    //invalid windows starting fileName strings
    private string[] invalidFileNamePrefixes = new string[]{"CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7",
    "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8","LPT9"};

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
        //set selected world in passData
        passData.SetWorldName(this.currentChosenWorld);
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

    /*
    method to correct filename so can be saved in windows (other platforms?)
    ", *, <, >, ?, \, |, /, :,
    not end with space or period
    reserved names (CON, PRN, ...)
    */
    public string CreateValidFileName(string name)
    {
        StringBuilder sb = new StringBuilder(name);

        //replace any invalid chars with '_'
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            sb.Replace(c, '_');
        }

        //add '_' before and after invalid word at begining of filename
        if(this.invalidFileNamePrefixes.Any(prefix => sb.ToString().StartsWith(prefix)))
        {
            sb.Insert(0, "_");
        }
        //Debug.Log(sb.ToString());
        return sb.ToString();
 
    }

    /*
    same as CreateValidFileName but return type null (show user fileName that will be in path)
    */
    public void CreateValidFileNameUI()
    {
        // string worldName = text.GetComponent<Text>().text;
        string worldName = this.worldNameInputText.GetComponent<InputField>().text;

        StringBuilder sb = new StringBuilder(worldName);

        //replace any invalid chars with '_'
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            sb.Replace(c, '_');
        }

        //add '_' before and after invalid word at begining of filename
        if(this.invalidFileNamePrefixes.Any(prefix => sb.ToString().ToUpper().StartsWith(prefix)))
        {
            sb.Insert(0, "_");
        }

        this.actualFilePathText.GetComponent<Text>().text = "File name: " + sb.ToString();
        //Debug.Log(sb.ToString());
    }
}
