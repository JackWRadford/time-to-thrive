using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public List<UIItem> uiItems = new List<UIItem>();
    public GameObject slotPrefab;
    //items parent
    public Transform slotPanel;

    void Awake()
    {
        for(int i = 0; i < 9; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    public void UpdateSlot(int slot, GameItem item)
    {
        uiItems[slot].UpdateItem(item);
    }

    public void AddNewItem(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == null), item);
    }

    public void RemoveItem(GameItem item)
    {
        UpdateSlot(uiItems.FindIndex(i=> i.item == item), null);
    }
}
