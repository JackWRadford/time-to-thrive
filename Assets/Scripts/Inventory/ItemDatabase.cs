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
            1,16,-1, false, false, false, false),
            //Apple
            new GameItem(1, "Apple", "Food",
            new Dictionary<string, int>{
                {"Nutrition", 5},
                {"Hydration", 1}
            },
            new Dictionary<string, int>{
            },
            1,16,-1, false, true, false, false),
            //Log
            new GameItem(2, "Log", "Material",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,8,-1, false, false, false, false),
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
            1,1,-1, false, false, false, false),
            //Fence
            new GameItem(4, "Fence", "Building",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                
            },
            1,16,-1, true, false, false, false),
            //WoodSword
            new GameItem(5, "WoodSword", "Weapon",
            new Dictionary<string, int>{
                {"Attack", 3},
            },
            new Dictionary<string, int>{
                {"Stick", 2},
                {"Log", 1}
            },
            1,1,-1, false, false, false, false),
            //RawBeef
            new GameItem(6, "RawBeef", "Food",
            new Dictionary<string, int>{
                {"Nutrition", 2},
                {"Poison", 4}
            },
            new Dictionary<string, int>{
            },
            1,16,-1, false, true, false, false),
            //Leather
            new GameItem(7, "Leather", "Material",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,16,-1, false, false, false, false),
            //Wall
            new GameItem(8, "Wall", "Building",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                
            },
            1,64,-1, true, false, false, false),
            //Roof
            new GameItem(9, "Roof", "Building",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                
            },
            1,64,-1, true, false, false, false),
            //Foundation
            new GameItem(10, "Foundation", "Building",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                
            },
            1,64,-1, true, false, false, false),
            //Door
            new GameItem(11, "Door", "Building",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                
            },
            1,64,-1, true, false, true, false),
            //Bed
            new GameItem(12, "Bed", "Utility",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
                
            },
            1,64,-1, true, false, false, true),
            //OakSapling
            new GameItem(13, "OakSapling", "Agriculture",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,4,-1, true, false, false, false),
            //SpruceSapling
            new GameItem(14, "SpruceSapling", "Agriculture",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,4,-1, true, false, false, false),
            //RainforestSapling
            new GameItem(15, "RainforestSapling", "Agriculture",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,4,-1, true, false, false, false),
            //RaspberryBush
            new GameItem(16, "RaspberryBush", "Agriculture",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,4,-1, true, false, false, false),
            //BlueberryBush
            new GameItem(17, "BlueberryBush", "Agriculture",
            new Dictionary<string, int>{
            },
            new Dictionary<string, int>{
            },
            1,4,-1, true, false, false, false),
            //Raspberry
            new GameItem(18, "Raspberry", "Food",
            new Dictionary<string, int>{
                {"Nutrition", 5},
            },
            new Dictionary<string, int>{
            },
            1,4,-1, false, true, false, false),
            //Blueberry
            new GameItem(19, "Blueberry", "Food",
            new Dictionary<string, int>{
                {"Nutrition", 3},
            },
            new Dictionary<string, int>{
            },
            1,4,-1, false, true, false, false),
        };
    }
}
