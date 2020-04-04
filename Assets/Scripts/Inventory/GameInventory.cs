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

        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
    }

    #endregion

    public List<GameItem> characterItems = new List<GameItem>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;
    public int capacity = 30;

    private UIItem selectedItem;

    void Start()
    {
        // GiveItem(1);
        // GiveItem(0);
        // RemoveItem(0);
        // GiveItem(0);
    }

    public void RemoveSelectedItemIfExists()
    {
        if(selectedItem != null)
        {
            //Remove item from inventory
            GameInventory.instance.RemoveSelectedItem(selectedItem.item);
            //Update selectedItem UI
            selectedItem.UpdateItem(null);
        }
    }

    public bool IsStackable(string itemName)
    {
        RemoveSelectedItemIfExists();
        //check if item to be picked up can be stacked
        for (int i = 0; i < this.characterItems.Count; i++)
        {
            if(characterItems[i].title == itemName)
            {
                if((characterItems[i].count >= 1)&&(characterItems[i].count < characterItems[i].maxCount))
                {
                    characterItems[i].count++;
                    //update UI for increase in stack count
                    inventoryUI.UpdateItemCount(characterItems[i]);
                    Debug.Log(characterItems[i].title + " " + characterItems[i].count);
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsFull()
    {
        RemoveSelectedItemIfExists();
        //check if all inventory slots are full
        if(characterItems.Count >= capacity){
            return true;
        }
        return false;
    }

    public void GiveItem(int id)
    {
        RemoveSelectedItemIfExists();
        GameItem itemToAdd = new GameItem(itemDatabase.GetItem(id));
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }
    public void GiveItem(string itemName)
    {
        RemoveSelectedItemIfExists();
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

    public bool CheckForAmountOfItem(string itemName, int amount)
    {
        int amountNeeded = amount;
        foreach (var item in this.characterItems)
        {
            if(item.title == itemName)
            {
                amountNeeded -= item.count;
            }
            if(amountNeeded <= 0)
            {
                return true;
            }
        }
        return false;
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
    public void RemoveSelectedItem(GameItem i)
    {
        GameItem itemToRemove = CheckForItem(i);

        if(itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            Debug.Log("Removed item: " + itemToRemove.title);
        }
    }

    //returns one of input item, adds it to inventory, lowers count of input
    public GameItem SplitOneOffItemStack(GameItem i)
    {
        GameItem singleItem = new GameItem(itemDatabase.GetItem(i.id));
        characterItems.Add(singleItem);
        return singleItem;
    }

    //remove certain amount of an item
    public void RemoveAmountOfItem(string itemName, int amount)
    {
        int amountToRemove = amount;
        foreach (var item in this.characterItems)
        {
            if(item.title == itemName)
            {
                int t = item.count;
                for (int i = 0; i < t; i++)
                {
                    item.count--;
                    amountToRemove--;
                    if(item.count <= 0)
                    {
                        RemoveItem(item);
                    }
                    else
                    {
                        inventoryUI.UpdateItemCount(item);
                    }
                    if(amountToRemove <= 0)
                    {
                        return;
                    }
                }
                inventoryUI.UpdateItemCount(item);
            }
        }
    }


    //check if item can be crafted from items in inventory
    public bool CanCraftItem(Dictionary<string,int> recipe)
    {
        foreach (var i in recipe)
        {
            Debug.Log(i.Value + " " + i.Key);
            //check for specified amount of an item
            if(!CheckForAmountOfItem(i.Key, i.Value))
            {
                return false;
            }
        }
        return true;
    }

    //try to craft the new item (remove recipe from inventory and add new item)
    public void CraftItemIfPossible(GameItem itemToCraft)
    {
        RemoveSelectedItemIfExists();
        if(CanCraftItem(itemToCraft.recipe))
        {
            if(!IsFull())
            {   
                //remove recipe items from inventory
                foreach (var item in itemToCraft.recipe)
                {
                    RemoveAmountOfItem(item.Key, item.Value);
                }

                //add new item to inventory
                GiveItem(itemToCraft.title);
            }
            else
            {
                Debug.Log("Inventory too full to add new item!");
            }
        }
    }
}
