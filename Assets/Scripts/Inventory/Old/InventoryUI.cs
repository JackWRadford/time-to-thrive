using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // public Transform itemsParent;
    // Inventory inventory;
    // InventorySlot[] slots;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     inventory = Inventory.instance;
    //     //subscribe to event (call UpdateUI whenever an item is added or removed)
    //     inventory.onItemAddedCallback += UpdateUIAddItem;

    //     slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    // }
    
    // //update inventory UI with new item/ new item count
    // void UpdateUIAddItem()
    // {
    //     for (int i = 0; i < inventory.items.Count; i++)
    //     {
    //         if(inventory.items[i] != null)
    //         {
    //             slots[inventory.items[i].slotNum].AddItem(inventory.items[i].item, inventory.items[i].count);
    //         }
    //     }
    // }
}
