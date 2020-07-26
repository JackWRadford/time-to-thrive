using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameItem gameItem;
    private PlayerController playerController;

    //offset to make pointer start inside box
    private Vector3 offset = new Vector3(-15,5,0);

    void Awake()
    {
        
    }

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        //set position to mouse position on right click event
        this.transform.position = Input.mousePosition + offset;
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
        GameObject drop = Resources.Load<GameObject>("PickUpItems/" + this.gameItem.title);
        for(int i = 0; i < this.gameItem.count; i++)
        {
            Instantiate(drop, playerController.GetPosInfrontOfPlayer(), Quaternion.identity);
        }
        //remove item from game inventory
        GameInventory.instance.RemoveItem(this.gameItem);

        gameObject.SetActive(false);
    }

    public void ConsumeItem()
    {
        //Add effects to player health/thurst/hunger
        if((this.gameItem.stats != null)&&(this.gameItem.consumable))
        {
            if(this.gameItem.stats.ContainsKey("Nutrition"))
            {
                playerController.IncrementHunger(this.gameItem.stats["Nutrition"]);
            }
            if(this.gameItem.stats.ContainsKey("Hydration"))
            {
                playerController.IncrementThurst(this.gameItem.stats["Hydration"]);
            }

                //remove item from game inventory
            if(this.gameItem.count < 2)
            {
                GameInventory.instance.RemoveItem(this.gameItem);
            }
            else if(this.gameItem.count > 1)
            {
                //decrement count of item by 1
                GameInventory.instance.RemoveAmountOfItem(this.gameItem, 1);
            }

            gameObject.SetActive(false);
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
