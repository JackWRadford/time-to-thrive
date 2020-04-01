using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInventory : MonoBehaviour
{
    #region Singleton 
    public static GameInventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("more than one instance of inventory found");
        }
        instance = this;
    }

    #endregion

    public List<GameItem> characterItems = new List<GameItem>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;
    public int capacity = 9;

    void Start()
    {
        // GiveItem(1);
        // GiveItem(0);
        // RemoveItem(0);
        // GiveItem(0);
    }

    public bool IsStackable(string itemName)
    {
        //check if item to be picked up can be stacked
        for (int i = 0; i < this.characterItems.Count; i++)
        {
            if(characterItems[i].title == itemName)
            {
                if((characterItems[i].count >= 1)&&(characterItems[i].count < characterItems[i].maxCount))
                {
                    characterItems[i].count++;
                    Debug.Log(characterItems[i].title + " " + characterItems[i].count);
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsFull(string itemName)
    {
        //check if all inventory slots are full
        if(characterItems.Count >= capacity){
            return true;
        }
        return false;
    }

    public void GiveItem(int id)
    {
        GameItem itemToAdd = new GameItem(itemDatabase.GetItem(id));
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }
    public void GiveItem(string itemName)
    {
        GameItem itemToAdd = new GameItem(itemDatabase.GetItem(itemName));
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public GameItem CheckForItem(int id)
    {
        return characterItems.Find(item=> item.id == id );
    }
    public GameItem CheckForItem(string itemName)
    {
        return characterItems.Find(item=> item.title == itemName );
    }
    public GameItem CheckForItem(GameItem i)
    {
        return characterItems.Find(item=> item == i );
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
    public void RemoveItem(string itemName)
    {
        GameItem itemToRemove = CheckForItem(itemName);

        if(itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Removed item: " + itemToRemove.title);
        }
    }
    public void RemoveItem(GameItem i)
    {
        GameItem itemToRemove = CheckForItem(i);

        if(itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Removed item: " + itemToRemove.title);
        }
    }
}
