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
    public int selectedSlot = 0;
    

    public static bool inventoryIsOpen = false;

    private UIItem selectedItem; 
    private Tooltip tooltip;

    void Awake()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();

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
            //uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    void Update()
    {
        if(!PauseMenu.GameIsPaused)
        {
            //check for 1-9 input and set selected slot
            if(!inventoryIsOpen)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    selectedSlot = 0;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha2))
                {
                    selectedSlot = 1;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha3))
                {
                    selectedSlot = 2;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha4))
                {
                    selectedSlot = 3;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha5))
                {
                    selectedSlot = 4;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha6))
                {
                    selectedSlot = 5;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha7))
                {
                    selectedSlot = 6;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha8))
                {
                    selectedSlot = 7;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha9))
                {
                    selectedSlot = 8;
                }
            }
            //change background of selected slot
            //uiItems[selectedSlot].SetSelectedSlot();

            if(Input.GetKeyDown(KeyCode.E))
            {
                if(inventoryIsOpen)
                {
                    Debug.Log("Close Inventory");
                    bagUI.SetActive(false);
                    tooltip.gameObject.SetActive(false);
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
                    tooltip.gameObject.SetActive(false);
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

    public GameItem GetSelectedItem()
    {
        return uiItems[selectedSlot].item;
    }
}
