using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public float hours;
    public float minutes;
    public float seconds;
    public float globalLightIntensity;
    public int daysPassed;

    public WorldData(Ambient a)
    {
        this.hours = a.currentHours;
        this.minutes = a.currentMinutes;
        this.seconds = a.currentSeconds;

        this.daysPassed = a.daysPassed;

        this.globalLightIntensity = a.globalLightIntensity;
    }
}
