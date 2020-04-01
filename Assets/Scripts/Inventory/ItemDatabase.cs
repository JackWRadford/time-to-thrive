using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<GameItem> items = new List<GameItem>();

    void Awake()
    {
        BuildDatabase();
    }

    public GameItem GetItem(int id)
    {
        return items.Find(item=> item.id == id);
    }
    public GameItem GetItem(string itemName)
    {
        return items.Find(item=> item.title == itemName);
    }

    void BuildDatabase()
    {
        items = new List<GameItem>(){
            //Stick
            new GameItem(0, "Stick", "Just a stick",
            new Dictionary<string, int>{
                {"Attack", 2}
            }),
            //Apple
            new GameItem(1, "Apple", "Eat me",
            new Dictionary<string, int>{
                {"Nutrition", 5},
                {"EdibleLife", 3}
            })
        };
    }
}
