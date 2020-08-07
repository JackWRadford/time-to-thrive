﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDetails : MonoBehaviour
{
    private ObjectManager objectManager = null;

    public bool canBeOver = false;
    public bool canBeUnder = false;

    //if being placed save to objects list (don't if being loaded)
    public bool placeing = false;

    //height in world coordinates
    public float height = 0;

    public bool isExternalConstruction = false;
    public bool isWall = false;
    public bool isFoundation = false;
    public bool isRoof = false;
    public bool isDoor = false;
    public bool isGroundLevel = false;

    //further tiles in each direction
    public bool isMultiPos = false;
    public List<Vector3> extraPositions;

    //fields for if objects can be placed in same tile as walls
    public bool canBePlacedInside = false;


    public int orientation = 0;

    Vector3 posMinusOffset = new Vector3();

    void Awake()
    {
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
    }

    void Start()
    {
        posMinusOffset = this.transform.position - new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(), this.GetComponent<PlacementOffset>().GetOffsetY(),0);
    }

    public void SetPlaceing(bool tof)
    {
        this.placeing = tof;
    }

    public void DestroyGO()
    {
        Destroy(this.gameObject);
    }

    //called to show player/other objects behind walls/roof/door
    public void SeeBehind()
    {
        //make object transparent
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        if((this.isRoof))
        {
            tmp.a = 0f;
        }
        else if((this.isWall)&&(!this.isGroundLevel))
        {
            tmp.a = 0f;
        }
        else if((this.isWall)&&(this.isGroundLevel))
        {
            //set sprite to alternative wall sprite
            this.GetComponent<SpriteRenderer>().sprite = this.GetComponent<WallController>().alternativeSprite;
        }
        else if(this.isDoor)
        {
            tmp.a = 0.3f;
        }
        transform.GetComponent<SpriteRenderer>().color = tmp;
    }
    public void AntiSeeBehind()
    {
        //make object sprite alpha 1.0f
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        if((this.isWall)&&(this.isGroundLevel))
        {
            //Debug.Log(this.orientation.ToString());
            //set sprite to normal wall sprite
            this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/" + "Wall")[this.orientation];
        }
        tmp.a = 1.0f;
        transform.GetComponent<SpriteRenderer>().color = tmp;
    }

    //check if player is behind wall
    private void OnTriggerStay2D(Collider2D other) {
        if(this.isExternalConstruction)
        {
            //make wall transparent
            //SeeBehind();
            objectManager.SeeBehindExternalContruction(this.posMinusOffset.x, this.posMinusOffset.y);
        }
    }

    //check if player exists from behind tree
    private void OnTriggerExit2D(Collider2D other) {
        if(this.isExternalConstruction)
        {
            //make wall sprite alpha 1.0f
            //AntiSeeBehind();
            objectManager.AntiSeeBehindExternalContruction(this.posMinusOffset.x, this.posMinusOffset.y);
        }
    }
}
