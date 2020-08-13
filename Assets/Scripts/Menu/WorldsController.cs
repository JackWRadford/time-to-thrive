using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldsController : MonoBehaviour
{
    public GameObject worldNameInputText;
    private PassData passData;

    private string worldName;

    void Awake()
    {
        passData = GameObject.Find("PassInfo").GetComponent<PassData>();
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
}
