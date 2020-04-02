using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public List<UIItem> uiItems = new List<UIItem>();
    public GameObject slotPrefab;
    //items parent
    public Transform slotPanel;
    public Transform bagSlotPanel;
    public Transform equipSlotPanel;
    public GameObject bagUI;

    public static bool inventoryIsOpen = false;

    private UIItem selectedItem; 

    void Awake()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();

        //populate slotPanel (hotbar inventory)
        for(int i = 0; i < 9; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
        //populate bagSlotPanel (bag inventory)
        for(int i = 0; i < 21; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(bagSlotPanel);
            uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
        //populate equipSlotPanel (player equipment)
        for(int i = 0; i < 3; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(equipSlotPanel);
            uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    void Update()
    {
        if(!PauseMenu.GameIsPaused)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(inventoryIsOpen)
                {
                    Debug.Log("Close Inventory");
                    bagUI.SetActive(false);
                    //Cursor.visible = false;
                    //check if an item is selected
                    if(selectedItem.item != null)
                    {
                        //Remove item from inventory
                        GameInventory.instance.RemoveSelectedItem(selectedItem.item);
                        //Update selectedItem UI
                        selectedItem.UpdateItem(null);
                    }
                    //unpause
                    Time.timeScale = 1f;
                    inventoryIsOpen = false;
                }
                else
                {
                    Debug.Log("Open Inventory");
                    bagUI.SetActive(true);
                    //Cursor.visible = true;
                    //pause
                    Time.timeScale = 0f;
                    inventoryIsOpen = true;
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(inventoryIsOpen)
                {
                    Debug.Log("Close Inventory");
                    bagUI.SetActive(false);
                    //Cursor.visible = false;
                    //check if an item is selected
                    if(selectedItem.item != null)
                    {
                        //Remove item from inventory
                        GameInventory.instance.RemoveSelectedItem(selectedItem.item);
                        //Update selectedItem UI
                        selectedItem.UpdateItem(null);
                    }
                    //unpause
                    Time.timeScale = 1f;
                    inventoryIsOpen = false;
                }
            }
        }
    }

    public void UpdateSlot(int slot, GameItem item)
    {
        uiItems[slot].UpdateItem(item);
    }

    public void UpdateItemCount(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == item), item);
    }

    public void AddNewItem(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == null), item);
    }

    public void RemoveItem(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == item), null);
    }

    public void RemoveSelectedItem(GameItem item)
    {
        selectedItem.UpdateItem(null);
    }
}
