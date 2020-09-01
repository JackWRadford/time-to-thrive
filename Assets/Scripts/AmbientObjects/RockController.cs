using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : Interactable, ILoadState
{
    
    public ObjectManager objectManager;
    public PlacementOffset placementOffset;
    public StackDetails stackDetails;

    public GameObject rockDropPUI;
    public GameObject oreDropPUI;
    public Sprite stageFull;
    public Sprite stageLess_1;
    public Sprite stageLess_2;
    public Sprite stageEmpty;

    public string objectTitle = "";

    [SerializeField]
    private const int FULL_HEALTH = 30;
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

            //make bush empty when placing
            SetCorrectSprite();
        }
        else
        {
            //same to go array when loading from data (already have data from save)
            SaveGO();
            SetCorrectSprite();
        }
    }

    public void SaveState()
    {
        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y- placementOffset.GetOffsetY();

        RockData rd = new RockData(this);
        if(objectManager.IsSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY, "Rock", rd);
        }
    }

    //save GO to list (data already saved and loaded)
    public void SaveGO()
    {
        //check object not already in that position (double save in same position)
        if(objectManager.IsGOSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            //Debug.Log("add GO" + this.positionMinusOffsetX + 0.5f.ToString() + this.positionMinusOffsetY + 0.5f.ToString());
            objectManager.AddObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY, "Rock", this.gameObject);
        }
    }
    
    //set state from saved state
    public void LoadState(dynamic data)
    {
        //load stage and set correct sprite depending on it
        this.stage = data.stage;
        this.health = data.health;
    }

    public void UpdateState()
    {
        //find and remove data from objects list
        objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
        //add updated data to objects list
        RockData rd = new RockData(this);
        rd.title = this.objectTitle;
        objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY,"Rock", rd);
    }

    //set correct sprite for current stage (full, less1, less2, empty)
    public void SetCorrectSprite()
    {
        switch(this.stage)
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = this.stageEmpty;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = this.stageLess_2;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = this.stageLess_1;
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = this.stageFull;
                break;
        }
    }

    public int GetHealth()
    {
        return this.health;
    }

    public override void NDInteract(GameObject go)
    {
        base.NDInteract(go);

        //mine rock and lower stage if possible
        // if((this.stage <= 3)&&(this.stage > 0))
        // {
        //     MineRock();
        // }
    }

    private void MineRock()
    {
        this.stage--;
        Vector3 rd = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
        Instantiate(rockDropPUI,transform.position + rd, Quaternion.identity);
        SetCorrectSprite();
        UpdateState();
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
                //check for StoneCutting stat
                if(go.GetComponent<PlayerController>().GetHeldItem().stats.ContainsKey("WoodCutting"))
                {
                    health-=go.GetComponent<PlayerController>().GetHeldItem().stats["WoodCutting"];

                    //check if health decreased enough to decrement stage and drop items
                    if(FULL_HEALTH-this.health >= FULL_HEALTH/3)
                    {
                        MineRock();
                    }

                }
                else
                {
                    //health-=go.GetComponent<PlayerController>().GetDefaultAttack();
                }
            }
            else
            {
                //health-=go.GetComponent<PlayerController>().GetDefaultAttack();
            }

        }
        if(health<=0)
        {
        //drop random amount and items and destroy gameObject

        
        objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
        objectManager.RemoveObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY);

        Destroy(gameObject);
        return;
        }
    }
}
