using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameItem
{
    public int id;
    public string title;
    public string description;
    //public Sprite icon;
    public Dictionary<string, int> stats = new Dictionary<string, int>();
    public Dictionary<string, int> recipe = new Dictionary<string, int>();
    public int count;
    public int maxCount;
    public int slot;

    public bool placeable;

    public GameItem(int id, string title, string description, Dictionary<string, int> stats, Dictionary<string, int> recipe, int count, int maxCount, int slot, bool placeable)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        //this.icon = Resources.Load<Sprite>("Sprites/Items/" + title);
        this.stats = stats;
        this.recipe = recipe;
        this.count = count;
        this.maxCount = maxCount;
        this.slot = slot;
        this.placeable = placeable;
    }

    //copy constructor
    public GameItem(GameItem item)
    {
        this.id = item.id;
        this.title = item.title;
        this.description = item.description;
        //this.icon = Resources.Load<Sprite>("Sprites/Items/" + item.title);
        this.stats = item.stats;
        this.recipe = item.recipe;
        this.count = item.count;
        this.maxCount = item.maxCount;
        this.slot = item.slot;
        this.placeable = item.placeable;
    }
}
