using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBushController : Interactable, ILoadState
{
    public ObjectManager objectManager;
    public PlacementOffset placementOffset;
    public StackDetails stackDetails;

    public GameObject berryBushPUI;
    public GameObject berryPUI;
    public Sprite berryBushFull;
    public Sprite berryBushLess_1;
    public Sprite berryBushLess_2;
    public Sprite berryBushEmpty;

    private int health = 10;
    //stages: 0-empty 1-less2 2-less1 3-full
    public int stage = 0;

    private float positionMinusOffsetX;
    private float positionMinusOffsetY;

    // Start is called before the first frame update
    void Start()
    {
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        placementOffset = this.gameObject.GetComponent<PlacementOffset>();
        stackDetails = this.gameObject.GetComponent<StackDetails>();
        
        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y - placementOffset.GetOffsetY();

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

    public void SaveState()
    {
        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y- placementOffset.GetOffsetY();

        BerryBushData bbd = new BerryBushData(this);
        if(objectManager.IsSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY, "BerryBush", bbd);
        }
    }

    //save GO to list (data already saved and loaded)
    public void SaveGO()
    {
        //check object not already in that position (double save in same position)
        if(objectManager.IsGOSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            //Debug.Log("add GO" + this.positionMinusOffsetX + 0.5f.ToString() + this.positionMinusOffsetY + 0.5f.ToString());
            objectManager.AddObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY, "BerryBush", this.gameObject);
        }
    }
    
    //set state from saved state
    public void LoadState(dynamic data)
    {
        //load stage and set correct sprite depending on it
        this.stage = data.stage;
    }

    public void UpdateState()
    {
        //find and remove data from objects list
        objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
        //add updated data to objects list
        BerryBushData bbd = new BerryBushData(this);
        objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY,"BerryBush",bbd);
    }

    //set correct sprite for current stage (full, less1, less2, empty)
    public void SetCorrectSprite()
    {
        
    }

    public int GetHealth()
    {
        return this.health;
    }

    public override void NDInteract(GameObject go)
    {
        base.NDInteract(go);

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
        //drop berryBush (drop berrys if relevant stage???)
        Vector3 rd = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
        Instantiate(berryBushPUI,transform.position + rd, Quaternion.identity);
        
        objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
        objectManager.RemoveObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY);

        Destroy(gameObject);
        return;
        }
    }
}
