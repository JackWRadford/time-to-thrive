using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject mainHUD;
    
    private UIItem selectedItem; 
    private Tooltip tooltip;
    private GameManager gameManager;

    //private bool mainHUDisIOpen = false;

    void Awake()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        //CloseMainHUD();
    }

    public void OpenMainHUD()
    {
        mainHUD.SetActive(true);
        //mainHUDisIOpen = true;
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
    }

    //open relevant section for TAB selected
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        //make gameManager aware that mouse is over main HUD UI 
        gameManager.mouseOverMainHUDUI = true;
        //Debug.Log("in");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //make gameManager aware that mouse is not over main HUD UI
        gameManager.mouseOverMainHUDUI = false;
        //Debug.Log("out");
    }
}
