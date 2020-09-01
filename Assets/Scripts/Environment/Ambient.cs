using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using UnityEngine.UI;
using System.Text;


public class Ambient : MonoBehaviour
{
    public GameObject globalLight;
    public GameObject timePanel;
    public string worldTime;
    public int daysPassed;
    private float timer = 0f;
    public float delay = 0.1f;
    public float currentHours = 12f;
    public float currentMinutes = 00f;
    public float currentSeconds = 00f;
    public float globalLightIntensity = 1f;
    private StringBuilder sbTime = new StringBuilder("");
    

    void Awake()
    {
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;
    }

    void Start()
    {
        globalLight.GetComponent<Light2D>().intensity = this.globalLightIntensity;
    }

    void Update()
    {
        this.worldTime = DateTime.Now.ToString("hh:mm:ss");

        timer += Time.deltaTime;
        if(timer >= delay)
        {
            timer = 0f;
            IncrementTime();
            UpdateTimePanel();
        }

        this.timePanel.GetComponent<Text>().text = this.sbTime.ToString();
    }

    public void IncrementTime()
    {
        this.currentSeconds++;
        if(this.currentSeconds >= 60f)
        {
            this.currentSeconds = 00f;
            this.currentMinutes++;
            if(this.currentMinutes >= 60f)
            {
                this.currentMinutes = 00f;
                this.currentHours++;
                if(this.currentHours >= 24)
                {
                    this.currentHours = 00f;
                    this.daysPassed++;

                    //midnight
                    ResetLight(true);
                }
                else if(this.currentHours == 12)
                {
                    //midday
                    ResetLight(false);
                }
                else if(this.currentHours < 12)
                {
                    //increase light
                    ChangeGlobalLight(true);
                }
                else if((this.currentHours > 12)&&(this.currentHours < 24))
                {
                    //decrease light
                    ChangeGlobalLight(false);
                }
            }
        }
    } 

    public void UpdateTimePanel()
    {
        //update new time on timePanel
        this.sbTime.Clear();
        this.sbTime.Append(this.currentHours.ToString().PadLeft(2, '0'));
        this.sbTime.Append(":");
        this.sbTime.Append(this.currentMinutes.ToString().PadLeft(2, '0'));
        // this.sbTime.Append(":");
        // this.sbTime.Append(this.currentSeconds.ToString().PadLeft(2, '0'));
    }

    public void SetLightIntensity()
    {
        globalLight.GetComponent<Light2D>().intensity = this.globalLightIntensity;
    }

    public void ChangeGlobalLight(bool increase)
    {
        if(increase)
        {
            globalLight.GetComponent<Light2D>().intensity += 0.0667f;
            this.globalLightIntensity = globalLight.GetComponent<Light2D>().intensity;
        }
        else if(!increase)
        {
            globalLight.GetComponent<Light2D>().intensity -= 0.0667f;
            this.globalLightIntensity = globalLight.GetComponent<Light2D>().intensity;
        }
    }

    public void ResetLight(bool night)
    {
        if(night)
        {
            globalLight.GetComponent<Light2D>().intensity = 0.2f;
            this.globalLightIntensity = globalLight.GetComponent<Light2D>().intensity;
        }
        else if(!night)
        {
            globalLight.GetComponent<Light2D>().intensity = 1f;
            this.globalLightIntensity = globalLight.GetComponent<Light2D>().intensity;
        }
    }

    void Save()
    {
        WorldData wd = new WorldData(this);
        SaveSystem.Save<dynamic>(wd, "ambient");
        Debug.Log("Saved Ambient");
    }

    void Load()
    {
        if(SaveSystem.SaveExists("ambient"))
        {
            Debug.Log("Ambient Save Exists");

            WorldData data = SaveSystem.Load<dynamic>("ambient");

            this.currentHours = data.hours;
            this.currentMinutes = data.minutes;
            this.currentSeconds = data.seconds;

            this.daysPassed = data.daysPassed;

            this.globalLightIntensity = data.globalLightIntensity;

            UpdateTimePanel();
            //SetLightIntensity();
        }
        else
        {
            Debug.Log("Ambient Save Doesn't Exist");
        }
    }
}
