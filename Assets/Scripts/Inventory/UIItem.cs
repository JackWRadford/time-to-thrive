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
    private Text stackCount;

    void Awake()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        spriteImage = GetComponent<Image>();
        stackCount = transform.GetChild(0).GetComponent<Text>();
        UpdateItem(null);
    }

    public void UpdateItem(GameItem item)
    {
        this.item = item;
        if(this.item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = item.icon;
            if(item.count > 1)
            {
                stackCount.text = item.count.ToString();
            }
            else
            {
                stackCount.text = null;
            }
        }
        else
        {
            spriteImage.color = Color.clear;
            stackCount.text = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //check for parent (don't allow click if in hotbar slotParent) (allow in actual inventory)
        //if(UIInventory.inventoryIsOpen)
        
            //debug
            if(Input.GetMouseButtonDown(2))
            {
                for (int i = 0; i < GameInventory.instance.characterItems.Count; i++)
                {
                    if(GameInventory.instance.characterItems[i] != null)
                    {
                        Debug.Log(GameInventory.instance.characterItems[i].title + " : " + GameInventory.instance.characterItems[i].count);
                    }
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                if(this.item != null)
                {
                    //if dragging an item
                    if(selectedItem.item != null)
                    {
                        //check if items can be stacked
                        if((this.item.title == selectedItem.item.title)&&(this.item.maxCount > 1))
                        {
                            if((this.item.count < this.item.maxCount))
                            {
                                Debug.Log("can stack");
                                int t = selectedItem.item.count;
                                for (int i = 0; i < t; i++)
                                {
                                    selectedItem.item.count--;
                                    this.item.count++;
                                    if(this.item.count == this.item.maxCount)
                                    {
                                        Debug.Log("maxed");
                                        UpdateItem(this.item);
                                        if(selectedItem.item.count <= 0)
                                        {
                                            GameInventory.instance.RemoveSelectedItem(selectedItem.item);
                                            selectedItem.UpdateItem(null);
                                        }
                                        else
                                        {
                                            selectedItem.UpdateItem(selectedItem.item);
                                        }
                                        return;
                                    }
                                    if(selectedItem.item.count <= 0)
                                    {
                                        Debug.Log("ran out");
                                        UpdateItem(this.item);
                                        GameInventory.instance.RemoveSelectedItem(selectedItem.item);
                                        selectedItem.UpdateItem(null);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //clone item in slot
                            GameItem clone = selectedItem.item;
                            //put item in slot as itembeing dragged
                            selectedItem.UpdateItem(this.item);
                            //put item that was bring dragged in slot
                            UpdateItem(clone);
                        }
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
                if((this.item != null)&&(selectedItem.item != null))
                {
                    //delete item
                    //GameInventory.instance.RemoveItem(this.item);
                    
                    //add one to stack
                    //check if items can be stacked
                    if((this.item.title == selectedItem.item.title)&&(this.item.maxCount > 1))
                    {
                        if((this.item.count < this.item.maxCount))
                        {
                            Debug.Log("can stack");
                            
                            selectedItem.item.count--;
                            this.item.count++;
                            if(selectedItem.item.count <= 0)
                            {
                                Debug.Log("ran out");
                                UpdateItem(this.item);
                                GameInventory.instance.RemoveSelectedItem(selectedItem.item);
                                selectedItem.UpdateItem(null);
                                return;
                            }
                            else
                            {
                                UpdateItem(this.item);
                                selectedItem.UpdateItem(selectedItem.item);
                            }
                            
                        }
                    }
                }
                else if(selectedItem != null)
                {
                    //check enough to split
                    if(selectedItem.item.count > 1)
                    {
                        //put one of dragged item in slot
                        UpdateItem(GameInventory.instance.SplitOneOffItemStack(selectedItem.item));
                        //decrement count of selected item
                        selectedItem.item.count--;
                        selectedItem.UpdateItem(selectedItem.item);
                    }
                }
            }
        
    }
}
