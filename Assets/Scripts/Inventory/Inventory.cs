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

    public class ItemCountCombo
    {
        public Item item;
        public int count;

        public int slotNum;

        public ItemCountCombo(Item i, int c, int sn)
        {
            item = i;
            count = c;
            slotNum = sn;
        }

        public void IncrementCount()
        {
            count = count + 1;
        }

        public void SetSlotNum(int sn)
        {
            slotNum = sn;
        }
    }

    //update UI when certain events happen
    public delegate void OnItemAdded();
    public OnItemAdded onItemAddedCallback;

    public delegate void OnItemRemoved();
    public OnItemRemoved onItemRemovedCallback;

    public int space = 9;

    public List<ItemCountCombo> items = new List<ItemCountCombo>();

    //slots used or not
    public bool[] used = new bool[]{false,false,false,false,false,false,false,false,false};

    public bool Add(Item item)
    {
        bool stacked = false;
        //check if any items can be stacked
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].item.stackable == true)
            {
                if(items[i].item.name == item.name)
                {
                    if(items[i].count < 2)
                    {
                        items[i].IncrementCount();
                        stacked = true;
                        break;
                    }
                }
            }
        }

        //if couldn't stack add to new slot 
        if(!stacked)
        {
            //check inventory is full
            if(items.Count >= space)
            {
                return false;
            }

            int slotIdx = 0;
            
            for (int i = 0; i < used.Length; i++)
            {
                if(!used[i])
                {
                    slotIdx = i;
                    used[i] = true;
                    break;
                }
            }
            
            ItemCountCombo s = new ItemCountCombo(item,1,slotIdx);
            items.Add(s);
        }

        //update UI
        if(onItemAddedCallback != null)
        {
            onItemAddedCallback.Invoke();
        }
        foreach (bool uon in used)
        {
            Debug.Log(uon);
        }

        return true;
    }

    public void Remove(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            //find item to be removed
            if(items[i].item == item)
            {
                Debug.Log(items[i].slotNum);
                used[items[i].slotNum] = false;
                items.RemoveAt(i);
                break;
            }
        }
        
        foreach (bool uon in used)
        {
            Debug.Log(uon);
        }
    }
}
