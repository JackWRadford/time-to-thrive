using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        //subscribe to event (call UpdateUI whenever an item is added or removed)
        inventory.onItemAddedCallback += UpdateUIAddItem;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //update inventory UI with new item/ new item count
    void UpdateUIAddItem()
    {
        // for (int i = 0; i < slots.Length; i++)
        // {
        //     if(i < inventory.items.Count)
        //     {
        //         slots[i].AddItem(inventory.items[i]);
        //     }else{
        //         slots[i].ClearSlot();
        //     }
        // }

        for (int i = 0; i < inventory.items.Count; i++)
        {
            for (int j = 0; j < slots.Length; j++)
            {
                //check if item can be stacked
                if(slots[j].GetItem() != null)
                {
                    if(slots[j].GetItem().stackable)
                    {
                        if(slots[j].GetItem().name == inventory.items[i].name)
                        {
                            slots[j].AddItem(inventory.items[i]);
                            return;
                        }
                    }
                }
                
                //if item can't be stacked add to free space
                if(slots[j].GetItem() == null)
                {
                    slots[j].AddItem(inventory.items[i]);
                    return;
                }
            }
        }
    }
}
