using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHTH : MonoBehaviour
{
    private GameObject playerHealth;
    private GameObject playerThurst;
    private GameObject playerHunger;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Sprite fullWater;
    public Sprite halfWater;
    public Sprite emptyWater;
    public Sprite fullFood;
    public Sprite halfFood;
    public Sprite emptyFood;


    void Awake()
    {
        playerHealth = GameObject.Find("PlayerHealth");
        playerThurst = GameObject.Find("PlayerThurst");
        playerHunger = GameObject.Find("PlayerHunger");
    }

    public void UpdateAll(float health, float thurst, float hunger)
    {
        //Debug.Log(health + " " + hunger + " " + thurst);
        UpdateHealth(health);
        UpdateThurst(thurst);
        UpdateHunger(hunger);
    }

    public void UpdateHealth(float health)
    {
        //Debug.Log(health);
        this.playerHealth.GetComponent<RectTransform>().localScale = new Vector3(health/PlayerController.maxHealth,1f,1f);
        // Image[] images = playerHealth.GetComponentsInChildren<Image>();

        // for (int i = 0; i < images.Length; i++)
        // {
        //     if(Mathf.RoundToInt(health/2) >= i)
        //     {
        //         //set full heart
        //         playerHealth.GetComponentsInChildren<Image>()[i].sprite = fullHeart;
        //     }
        //     else if((Mathf.RoundToInt(health/2) == i-1)&&(health % 2 != 0))
        //     {
        //         playerHealth.GetComponentsInChildren<Image>()[i].sprite = halfHeart;
        //     }
        //     else
        //     {
        //         playerHealth.GetComponentsInChildren<Image>()[i].sprite = emptyHeart;
        //     }
        // }
    }
    public void UpdateThurst(float thurst)
    {
        //Debug.Log(thurst);
        this.playerThurst.GetComponent<RectTransform>().localScale = new Vector3(thurst/PlayerController.maxThurst,1f,1f);
        // Image[] images = playerThurst.GetComponentsInChildren<Image>();

        // for (int i = 0; i < images.Length; i++)
        // {
        //     if(Mathf.RoundToInt(thurst/2) >= i)
        //     {
        //         //set full heart
        //         playerThurst.GetComponentsInChildren<Image>()[i].sprite = fullWater;
        //     }
        //     else if((Mathf.RoundToInt(thurst/2) == i-1)&&(thurst % 2 != 0))
        //     {
        //         playerThurst.GetComponentsInChildren<Image>()[i].sprite = halfWater;
        //     }
        //     else
        //     {
        //         playerThurst.GetComponentsInChildren<Image>()[i].sprite = emptyWater;
        //     }
        // }
        
    }
    public void UpdateHunger(float hunger)
    {
        //Debug.Log(hunger);
        this.playerHunger.GetComponent<RectTransform>().localScale = new Vector3(hunger/PlayerController.maxHunger,1f,1f);
        // Image[] images = playerHunger.GetComponentsInChildren<Image>();

        // for (int i = 0; i < images.Length; i++)
        // {
        //     if(Mathf.RoundToInt(hunger/2) >= i)
        //     {
        //         //set full heart
        //         playerHunger.GetComponentsInChildren<Image>()[i].sprite = fullFood;
        //     }
        //     else if((Mathf.RoundToInt(hunger/2) == i-1)&&(hunger % 2 != 0))
        //     {
        //         playerHunger.GetComponentsInChildren<Image>()[i].sprite = halfFood;
        //     }
        //     else
        //     {
        //         playerHunger.GetComponentsInChildren<Image>()[i].sprite = emptyFood;
        //     }
        // }
        
    }
}
