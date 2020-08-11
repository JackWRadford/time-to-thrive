using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject mainHUD;
    private UIItem selectedItem; 
    private Tooltip tooltip;
    private GameManager gameManager;

    private GameObject currentOpenSection;

    public GameObject inventorySection;
    public GameObject craftingSection;
    public GameObject researchSection;
    public GameObject storeSection;

    

    void Awake()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        inventorySection.SetActive(false);
        
    }

    public void OpenMainHUD()
    {
        ChangeHUDSection(inventorySection);
        mainHUD.SetActive(true);
        // if(currentOpenSection != null)
        // {
        //     currentOpenSection.SetActive(false);
        //     currentOpenSection = null;
        // }
    }

    public void CloseMainHUD()
    {
        tooltip.gameObject.SetActive(false);
        mainHUD.SetActive(false);
        if(selectedItem.item != null)
        {
            //Remove item from inventory
            GameInventory.instance.RemoveSelectedItem(selectedItem.item);
            //Update selectedItem UI
            selectedItem.UpdateItem(null);
        }
        if(currentOpenSection != null)
        {
            currentOpenSection.SetActive(false);
            currentOpenSection = null;
        }
    }

    //open relevant section for TAB selected
    public void ExpandHUDSection(Button btn)
    {
        Debug.Log(btn.name);
        switch (btn.name)
        {
            case "InventoryTAB":
                ChangeHUDSection(inventorySection);
                break;

            case "craftingTAB":
                ChangeHUDSection(craftingSection);
                break;
            
            case "researchTAB":
                ChangeHUDSection(researchSection);
                break;

            case "storeTAB":
                ChangeHUDSection(storeSection);
                break;

            default:
                Debug.Log("No section found");
                break;
        }
    }

    //logic for opening and closing correct HUD sections
    public void ChangeHUDSection(GameObject section)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        //make gameManager aware that mouse is over main HUD UI 
        gameManager.mouseOverMainHUDUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //make gameManager aware that mouse is not over main HUD UI
        gameManager.mouseOverMainHUDUI = false;
    }
}
