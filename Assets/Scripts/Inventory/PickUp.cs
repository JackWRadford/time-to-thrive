﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUp : Interactable
{
    //scriptable item to be added to inventory
    //public GameItem item;
    public string itemName;
    

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Player"))
        {
            //Add item to inventory if enough space
            if(GameInventory.instance.IsStackable(itemName))
            {
                Destroy(gameObject);
            }
            else if(!GameInventory.instance.IsFull(itemName))
            {
                GameInventory.instance.GiveItem(itemName);
                Destroy(gameObject);
            }
        }
    }
}
