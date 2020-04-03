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

    //start number (1: stackable, 0: non-stackable), max stack
    void BuildDatabase()
    {
        items = new List<GameItem>(){
            //Stick
            new GameItem(0, "Stick", "",
            new Dictionary<string, int>{
                {"Attack", 2}
            },1,8),
            //Apple
            new GameItem(1, "Apple", "",
            new Dictionary<string, int>{
                {"Nutrition", 5},
                {"Hydration", 1}
            },0,1),
            //Log
            new GameItem(2, "Log", "",
            new Dictionary<string, int>{
            },1,2)
        };
    }
}
