using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUp : Interactable
{
    //scriptable item to be added to inventory
    public Item item;
    

    private void OnTriggerEnter2D(Collider2D other) {
        //item.newGuid();
        item.id = Guid.NewGuid();
        if(other.CompareTag("Player"))
        {
            //Add to inventory if enough space


            Debug.Log("PickUp " + item.name);
            
            bool wasPickedUp = Inventory.instance.Add(item);
            if(wasPickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
