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

    //start number (1: stackable, 0: non-stackable), max stack, slot (-1 to start), placeable (bool)
    void BuildDatabase()
    {
        items = new List<GameItem>(){
            //Stick
            new GameItem(0, "Stick", "Misc",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,16,-1, false, false),
            //Apple
            new GameItem(1, "Apple", "Food",
            new Dictionary<string, int>{
                {"Nutrition", 5},
                {"Hydration", 1}
            },
            new Dictionary<string, int>{
            },
            1,16,-1, false, true),
            //Log
            new GameItem(2, "Log", "Material",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,8,-1, false, false),
            //WoodAxe
            new GameItem(3, "WoodAxe", "Tool",
            new Dictionary<string, int>{
                {"Attack", 2},
                {"WoodCutting", 2}
            },
            new Dictionary<string, int>{
                {"Stick", 2},
                {"Log", 1}
            },
            1,1,-1, false, false),
            //Fence
            new GameItem(4, "Fence", "Building",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                {"Stick", 3},
            },
            1,16,-1, true, false),
            //WoodSword
            new GameItem(5, "WoodSword", "Weapon",
            new Dictionary<string, int>{
                {"Attack", 3},
            },
            new Dictionary<string, int>{
                {"Stick", 2},
                {"Log", 1}
            },
            1,1,-1, false, false),
            //RawBeef
            new GameItem(6, "RawBeef", "Food",
            new Dictionary<string, int>{
                {"Nutrition", 2},
                {"Poison", 4}
            },
            new Dictionary<string, int>{
            },
            1,16,-1, false, true),
            //Leather
            new GameItem(7, "Leather", "Material",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,16,-1, false, false),
            //Wall
            new GameItem(8, "Wall", "Building",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                {"Log", 3}
            },
            1,64,-1, true, false),
        };
    }
}
