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

    public class Slot
    {
        public Item item;
        public int count;

        public Slot(Item i, int c)
        {
            item = i;
            count = c;
        }

        public void IncrementCount()
        {
            count = count + 1;
        }
    }

    //update UI when certain events happen
    public delegate void OnItemAdded();
    public OnItemAdded onItemAddedCallback;

    public delegate void OnItemRemoved();
    public OnItemRemoved onItemRemovedCallback;

    public int space = 9;
    //public List<Item> items = new List<Item>();
    public List<Slot> items = new List<Slot>();

    public bool Add(Item item)
    {
        bool stacked = false;
        //check if any items can be stacked
        for (int i = 0; i < items.Count; i++)
        {
            // if(items[i].stackable == true)
            // {
            //     if(items[i].name == item.name)
            //     {
            //         //items[i].count++;
            //         stacked = true;
            //     }
            // }
            if(items[i].item.stackable == true)
            {
                if(items[i].item.name == item.name)
                {
                    //items[i].count++;
                    items[i].IncrementCount();
                    stacked = true;
                }
            }
        }

        //check inventory is full
        if(items.Count >= space)
        {
            return false;
        }
        //if couldn't stack add to new slot 
        if(!stacked)
        {
            // items.Add(item);
            Slot s = new Slot(item,1);
            items.Add(s);
        }

        //update UI
        if(onItemAddedCallback != null)
        {
            onItemAddedCallback.Invoke();
        }

        return true;
    }

    public void Remove(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].item == item)
            {
                items.RemoveAt(i);
            }
        }
        //items.Remove(item);

        //update UI
        // if(onItemRemovedCallback != null)
        // {
        //     onItemRemovedCallback.Invoke();
        // }
    }
}
