using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallController : Interactable, ILoadState
{
    private ObjectManager objectManager;
    public GameObject wall;
    public PlacementOffset placementOffset;

    public int health = 1;
    public int orientation = 0;

    private float positionMinusOffsetX;
    private float positionMinusOffsetY;

    void Start()
    {   
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        placementOffset = this.gameObject.GetComponent<PlacementOffset>();
        SaveState();
    }

    //update state to be saved
    public void SaveState()
    {
        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y- placementOffset.GetOffsetY();

        WallData wd = new WallData(this);
        //check object not already in that position (double save in same position)
        if(objectManager.IsSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY))
        {
            objectManager.AddObject(this.positionMinusOffsetX, this.positionMinusOffsetY, "Wall", wd);
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
            objectManager.RemoveObject(wd.position[0], wd.position[1]);

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
