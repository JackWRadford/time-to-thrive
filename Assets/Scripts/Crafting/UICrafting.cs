using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICrafting : MonoBehaviour
{
    //private bool craftingIsOpen = false;
    private GameObject currentOpenSection;
    public GameObject craftingUI;
    public ItemDatabase itemDatabase;
    
    public GameObject toolsSection;
    public GameObject weaponsSection;
    public GameObject armourSection;
    public GameObject buildSection;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OpenCrafting()
    {
        craftingUI.SetActive(true);
        //PlayerController.SetAllowedToMove(false);
        //if closing close all crafting sections
        if(currentOpenSection != null)
        {
            currentOpenSection.SetActive(false);
            currentOpenSection = null;
        }
    }

    public void CloseCrafting()
    {
        craftingUI.SetActive(false);
        //PlayerController.SetAllowedToMove(true);
        //if closing close all crafting sections
        if(currentOpenSection != null)
        {
            currentOpenSection.SetActive(false);
            currentOpenSection = null;
        }
    }

    //open relevant expanded crafting section
    public void ExpandCraftingUI(Button btn)
    {
        switch (btn.name)
        {
            case "Tools":
                ChangeSection(toolsSection);
                break;

            case "Weapons":
                ChangeSection(weaponsSection);
                break;
            
            case "Armour":
                ChangeSection(armourSection);
                break;

            case "Building":
                ChangeSection(buildSection);
                break;

            default:
                Debug.Log("No section found");
                break;
        }
    }

    //logic for opening and closing correct crafting sections
    public void ChangeSection(GameObject section)
    {
        if((currentOpenSection != null)&&(currentOpenSection != section))
        {
            currentOpenSection.SetActive(false);
            currentOpenSection = section;
            section.SetActive(!section.activeInHierarchy);
        }
        else if(currentOpenSection == section)
        {
            //currentOpenSection = null;
        }
        else
        {
            currentOpenSection = section;
            section.SetActive(!section.activeInHierarchy);
        }
        // section.SetActive(!section.activeInHierarchy);
    }

    //try to craft selected item
    public void CraftItem(string itemToCraft)
    {
        //Debug.Log("craft: " + itemToCraft);
        //get gameItem from itemName
        GameItem item = itemDatabase.GetItem(itemToCraft);

        // check if item has a recipe
        if(item.recipe != null)
        {
            //try to craft item
            GameInventory.instance.CraftItemIfPossible(item);

            //Debug.Log(GameInventory.instance.CanCraftItem(item.recipe));
        }
    }
}
