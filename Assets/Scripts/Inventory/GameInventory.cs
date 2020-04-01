using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInventory : MonoBehaviour
{
    public List<GameItem> characterItems = new List<GameItem>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;

    void Start()
    {
        GiveItem(1);
        GiveItem(0);
        GiveItem(1);
        GiveItem(1);
        RemoveItem(0);
        GiveItem(0);
    }

    public void GiveItem(int id)
    {
        GameItem itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }
    public void GiveItem(string itemName)
    {
        GameItem itemToAdd = itemDatabase.GetItem(itemName);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public GameItem CheckForItem(int id)
    {
        return characterItems.Find(item=> item.id == id );
    }

    public void RemoveItem(int id)
    {
        GameItem itemToRemove = CheckForItem(id);

        if(itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Removed item: " + itemToRemove.title);
        }
    }
}
