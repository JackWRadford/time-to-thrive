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

    void Awake()
    {
        playerHealth = GameObject.Find("PlayerHealth");
        playerThurst = GameObject.Find("PlayerThurst");
        playerHunger = GameObject.Find("PlayerHunger");
    }

    public void UpdateHealth(int health)
    {
        Image[] images = playerHealth.GetComponentsInChildren<Image>();

        for (int i = 1; i < images.Length + 1; i++)
        {
            if(health >= i)
            {
                //set full heart
                playerHealth.GetComponentsInChildren<Image>()[i].sprite = fullHeart;
            }
            else
            {
                //set empty heart
                playerHealth.GetComponentsInChildren<Image>()[i].sprite = emptyHeart;
            }
        }
        
    }
}
