﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallController : Interactable, ILoadState
{
    private ObjectManager objectManager;
    public GameObject wall;
    public PlacementOffset placementOffset;
    public StackDetails stackDetails;

    public int health = 1;
    public int orientation = 0;

    private float positionMinusOffsetX;
    private float positionMinusOffsetY;

    //bools for if it canBeUnder / canbeOver (stacking)
    // public bool canBeUnder = true;
    // public bool canBeOver = true;

    void Start()
    {   
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        placementOffset = this.gameObject.GetComponent<PlacementOffset>();
        stackDetails = this.gameObject.GetComponent<StackDetails>();

        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y- placementOffset.GetOffsetY();
        
        //Debug.Log(stackDetails.placeing.ToString());
        if(stackDetails.placeing)
        {
            //save to data and go list when placeing
            stackDetails.SetPlaceing(false);
            SaveState();
            SaveGO();
        }
        else
        {
            //same to go array when loading from data (already have data from save)
            SaveGO();
        }
    }

    //update state to be saved
    public void SaveState()
    {
        WallData wd = new WallData(this);
        //check object not already in that position (double save in same position)
        if(objectManager.IsSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            Debug.Log("add DATA" + this.positionMinusOffsetX + 0.5f.ToString() + this.positionMinusOffsetY + 0.5f.ToString());
            objectManager.AddObjectData(this.positionMinusOffsetX, this.positionMinusOffsetY, "Wall", wd);
        }
    }

    //save GO to list (data already saved and loaded)
    public void SaveGO()
    {
        //check object not already in that position (double save in same position)
        if(objectManager.IsGOSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            Debug.Log("add GO" + this.positionMinusOffsetX + 0.5f.ToString() + this.positionMinusOffsetY + 0.5f.ToString());
            objectManager.AddObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY, "Wall", this.gameObject);
        }
    }

    //set state from saved state
    public void LoadState(dynamic data)
    {
        
    }

    public override void Interact(GameObject go)
    {
        base.Interact(go);
        
        //check playerController exists
        if(go.GetComponent<PlayerController>() != null)
        {
            //check player is holding an item
            if(go.GetComponent<PlayerController>().GetHeldItem() != null)
            {
                //check for WoodCutting stat
                if(go.GetComponent<PlayerController>().GetHeldItem().stats.ContainsKey("WoodCutting"))
                {
                    health-=go.GetComponent<PlayerController>().GetHeldItem().stats["WoodCutting"];
                }
                else
                {
                    health-=go.GetComponent<PlayerController>().GetDefaultAttack();
                }
            }
            else
            {
                health-=go.GetComponent<PlayerController>().GetDefaultAttack();
            }

        }
        if(health<=0)
        {
            //drop wall
            Instantiate(wall,transform.position, Quaternion.identity);
            
            //remove from objectManager list
            WallData wd = new WallData(this);
            objectManager.RemoveObject(this.positionMinusOffsetX, this.positionMinusOffsetY);
            objectManager.RemoveObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY);

            Destroy(gameObject);

            return;
        }
    }
    
    //rotate funtion to increment or decrement orientation
    public void Rotate(bool clockWise)
    {
        if(clockWise)
        {
            if(this.orientation < 3)
            {
                this.orientation++;
            }
            else
            {
                this.orientation = 0;
            }
        }
        else if(!clockWise)
        {
            if(this.orientation > 0)
            {
                this.orientation--;
            }
            else
            {
                this.orientation = 3;
            }
        }
        updateWall();
    }

    //method to update sprite and bo collider (prefab ?)
    public void updateWall()
    {
        Sprite[] allWoodWalls = Resources.LoadAll<Sprite>("Sprites/Placeable/Wall");

        GetComponent<SpriteRenderer>().sprite = allWoodWalls[this.orientation];
    }
}
