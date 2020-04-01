using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIItem : MonoBehaviour, IPointerDownHandler
{
    public GameItem item;
    private Image spriteImage;
    private UIItem selectedItem;

    void Awake()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
    }

    public void UpdateItem(GameItem item)
    {
        this.item = item;
        if(this.item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = item.icon;
        }
        else
        {
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(this.item != null)
            {
                //if dragging an item
                if(selectedItem.item != null)
                {
                    //clone item in slot
                    // not new GameItem ???
                    GameItem clone = selectedItem.item;
                    //put item in slot as itembeing dragged
                    selectedItem.UpdateItem(this.item);
                    //put item that was bring dragged in slot
                    UpdateItem(clone);
                }
                else
                {
                    //not dragging an item
                    selectedItem.UpdateItem(this.item);
                    UpdateItem(null);
                }
            }
            //dragging item and no item in clicked slot
            else if(selectedItem.item != null)
            {
                //put dragged item in slot
                UpdateItem(selectedItem.item);
                selectedItem.UpdateItem(null);
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            if(this.item != null)
            {
                //delete item
                GameInventory.instance.RemoveItem(this.item);
            }
        }
    }
}
