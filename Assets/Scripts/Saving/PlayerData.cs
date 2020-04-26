using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position;
    public int health;
    public int thurst;
    public int hunger;

    public PlayerData(PlayerController player )
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        this.health = player.GetHealth();
        this.thurst = player.GetThurst();
        this.hunger = player.GetHunger();
    }
}
