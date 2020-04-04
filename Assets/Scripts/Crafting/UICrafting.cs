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
    
    public GameObject toolsSection;
    public GameObject weaponsSection;
    public GameObject armourSection;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            craftingUI.SetActive(!craftingUI.activeInHierarchy);
            //if closing close all crafting sections
            if(currentOpenSection != null)
            {
                currentOpenSection.SetActive(false);
                currentOpenSection = null;
            }
            
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
        }
        else if(currentOpenSection == section)
        {
            currentOpenSection = null;
        }
        else
        {
            currentOpenSection = section;
        }
        section.SetActive(!section.activeInHierarchy);
    }
}
