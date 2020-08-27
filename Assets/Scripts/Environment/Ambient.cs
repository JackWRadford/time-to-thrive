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
    private string worldTime;
    private int daysPassed;
    private float timer = 0f;
    public float delay = 1f;
    private float currentHours = 00f;
    private float currentMinutes = 00f;
    private float currentSeconds = 00f;
    private StringBuilder sbTime = new StringBuilder("");
    

    void Awake()
    {

    }

    void Start()
    {
        globalLight.GetComponent<Light2D>().intensity = 1.0f;
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
        this.sbTime.Append(":");
        this.sbTime.Append(this.currentSeconds.ToString().PadLeft(2, '0'));
    }
}
