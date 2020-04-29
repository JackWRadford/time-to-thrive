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
    private int selectedSlot = 0;
    private int prevSelectedSlot = -1;
    

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

    public void SetSlotSelected(int s)
    {
        prevSelectedSlot = selectedSlot;
        selectedSlot = s;
    }

    public void UpdateSelected()
    {
        uiItems[selectedSlot].SetSelected(true);
        uiItems[selectedSlot].SetSelectedSlot();
        if(prevSelectedSlot != -1)
        {
            uiItems[prevSelectedSlot].SetSelected(false);
            uiItems[prevSelectedSlot].UnsetSelectedSlot();
        }
    }

    void Update()
    {
        if(!PauseMenu.GameIsPaused)
        {
            //check for 1-9 input and set selected slot
            //if(!inventoryIsOpen)
            //{
                if((Input.GetKeyDown(KeyCode.Alpha1))&&(selectedSlot != 0))
                {
                    SetSlotSelected(0);
                    //Debug.Log("0");
                }
                else if((Input.GetKeyDown(KeyCode.Alpha2))&&(selectedSlot != 1))
                {
                    SetSlotSelected(1);
                    //Debug.Log("1");
                }
                else if((Input.GetKeyDown(KeyCode.Alpha3))&&(selectedSlot != 2))
                {
                    SetSlotSelected(2);
                }
                else if((Input.GetKeyDown(KeyCode.Alpha4))&&(selectedSlot != 3))
                {
                    SetSlotSelected(3);
                }
                else if((Input.GetKeyDown(KeyCode.Alpha5))&&(selectedSlot != 4))
                {
                    SetSlotSelected(4);
                }
                else if((Input.GetKeyDown(KeyCode.Alpha6))&&(selectedSlot != 5))
                {
                    SetSlotSelected(5);
                }
                else if((Input.GetKeyDown(KeyCode.Alpha7))&&(selectedSlot != 6))
                {
                    SetSlotSelected(6);
                }
                else if((Input.GetKeyDown(KeyCode.Alpha8))&&(selectedSlot != 7))
                {
                    SetSlotSelected(7);
                }
                else if((Input.GetKeyDown(KeyCode.Alpha9))&&(selectedSlot != 8))
                {
                    SetSlotSelected(8);
                }
                //update selected slot      
                UpdateSelected();
            //}
        }
    }

    public void OpenBag()
    {
        //Debug.Log("Open Inventory");
        bagUI.SetActive(true);
        //PlayerController.SetAllowedToMove(false);
        inventoryIsOpen = true;
    }

    public void CloseBag()
    {
        //Debug.Log("Close Inventory");
        tooltip.gameObject.SetActive(false);
        bagUI.SetActive(false);
        //PlayerController.SetAllowedToMove(true);
        if(selectedItem.item != null)
        {
            //Remove item from inventory
            GameInventory.instance.RemoveSelectedItem(selectedItem.item);
            //Update selectedItem UI
            selectedItem.UpdateItem(null);
        }
    }

    public void UpdateSlot(int slot, GameItem item)
    {
        uiItems[slot].UpdateItem(item);
        if(item != null)
        {
            item.slot = slot;
            //Debug.Log(item.title + " is in " + item.slot);
        }
    }

    public void UpdateItemCount(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == item), item);
    }

    public void AddNewItem(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == null), item);
        //UpdateSelectedItem(uiItems.FindIndex(i=> i.item == item));
    }

    public void RemoveItem(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == item), null);
        item.slot = -1;
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
