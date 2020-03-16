using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUp : Interactable
{
    //scriptable item to be added to inventory
    public Item item;
    

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Player"))
        {
            //Add instance to inventory if enough space
            bool wasPickedUp = Inventory.instance.Add(Instantiate(item));
            if(wasPickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
