using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton 
    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("more than one instance of inventory found");
        }
        instance = this;
    }

    #endregion

    //update UI when certain events happen
    public delegate void OnItemAdded();
    public OnItemAdded onItemAddedCallback;

    public delegate void OnItemRemoved();
    public OnItemRemoved onItemRemovedCallback;

    public int space = 9;
    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if(items.Count >= space)
        {
            return false;
        }
        items.Add(item);

        //update UI
        if(onItemAddedCallback != null)
        {
            onItemAddedCallback.Invoke();
        }

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        //update UI
        // if(onItemRemovedCallback != null)
        // {
        //     onItemRemovedCallback.Invoke();
        // }
    }
}
