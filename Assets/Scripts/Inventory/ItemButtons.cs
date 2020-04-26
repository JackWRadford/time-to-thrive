using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtons : MonoBehaviour
{
    private GameItem gameItem;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        //set position to mouse position on right click event
        this.transform.position = Input.mousePosition;
    }

    public void SetGameItem(GameItem gameItem)
    {
        this.gameItem = gameItem;
    }

    public GameItem GetGameItem()
    {
        return this.gameItem;
    }

    public void DropItem()
    {
        //Instantiate item infront of player

        //remove item from game inventory
        GameInventory.instance.RemoveItem(this.gameItem);

        gameObject.SetActive(false);
    }

    public void ConsumeItem()
    {
        //Add effects to player health/thurst/hunger

        //remove item from game inventory
        GameInventory.instance.RemoveItem(this.gameItem);

        gameObject.SetActive(false);
    }
}
