using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class FenceController : Interactable, ILoadState
{
    private ObjectManager objectManager;
    public GameObject fence;
    public PlacementOffset placementOffset;

    public int health = 1;
    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;

    private float positionMinusOffsetX;
    private float positionMinusOffsetY;
    
    void Awake()
    {

        //Debug.Log("Place fence");
        checkConnections();

        // objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        // UpdateState();
    }

    void Start()
    {
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        placementOffset = this.gameObject.GetComponent<PlacementOffset>();
        SaveState();
    }

    public void checkConnections()
    {
        //on placement raycast UDLR to check for adjacent fences
        //RaycastHit2D[] hitsU = Physics2D.RaycastAll(rb2D.position + Vector2.up * 0.2f, 0.2f, lookDirection, 0.5f, LayerMask.GetMask("InteractiveObjects"));
        //Debug.Log(this.transform.position);
        RaycastHit2D[] hitsU = Physics2D.RaycastAll(this.transform.position + new Vector3(0, 0.3f, 0), new Vector2(0,1), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsU)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Up");
                    hit.collider.GetComponent<FenceController>().down = true;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    this.up = true;
                }
            }
        }
        RaycastHit2D[] hitsD = Physics2D.RaycastAll(this.transform.position + new Vector3(0, -0.3f, 0), new Vector2(0,-1), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsD)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Down");
                    hit.collider.GetComponent<FenceController>().up = true;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    this.down = true;
                }
            }
        }
        RaycastHit2D[] hitsL = Physics2D.RaycastAll(this.transform.position + new Vector3(-0.3f, 0.1f, 0), new Vector2(-1,0), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsL)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Left");
                    hit.collider.GetComponent<FenceController>().right = true;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    this.left = true;
                }
            }
        }
        RaycastHit2D[] hitsR = Physics2D.RaycastAll(this.transform.position + new Vector3(0.3f, 0.1f, 0), new Vector2(1,0), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsR)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Right");
                    hit.collider.GetComponent<FenceController>().left = true;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    this.right = true;
                }
            }
        }
        UpdateConnections();
    }

    public void checkConnectionsRemove()
    {
        //on placement raycast UDLR to check for adjacent fences
        //RaycastHit2D[] hitsU = Physics2D.RaycastAll(rb2D.position + Vector2.up * 0.2f, 0.2f, lookDirection, 0.5f, LayerMask.GetMask("InteractiveObjects"));
        //Debug.Log(this.transform.position);
        RaycastHit2D[] hitsU = Physics2D.RaycastAll(this.transform.position + new Vector3(0, 0.3f, 0), new Vector2(0,1), 0.7f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsU)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Up");
                    hit.collider.GetComponent<FenceController>().down = false;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    //this.up = false;
                }
            }
        }
        RaycastHit2D[] hitsD = Physics2D.RaycastAll(this.transform.position + new Vector3(0, -0.3f, 0), new Vector2(0,-1), 0.7f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsD)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Down");
                    hit.collider.GetComponent<FenceController>().up = false;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    //this.down = false;
                }
            }
        }
        RaycastHit2D[] hitsL = Physics2D.RaycastAll(this.transform.position + new Vector3(-0.3f, 0.1f, 0), new Vector2(-1,0), 0.7f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsL)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Left");
                    hit.collider.GetComponent<FenceController>().right = false;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    //this.left = false;
                }
            }
        }
        RaycastHit2D[] hitsR = Physics2D.RaycastAll(this.transform.position + new Vector3(0.3f, 0.1f, 0), new Vector2(1,0), 0.7f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsR)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    //Debug.Log("Fence Right");
                    hit.collider.GetComponent<FenceController>().left = false;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    //this.right = false;
                }
            }
        }
        UpdateConnections();
    }

    //update state to be saved
    public void SaveState()
    {
        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y- placementOffset.GetOffsetY();

        FenceData fd = new FenceData(this);
        //check object not already in that position (double save in same position)
        if(objectManager.IsSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY))
        {
            //Debug.Log("Add fence");
            objectManager.AddObject(this.positionMinusOffsetX,this.positionMinusOffsetY, "Fence", fd);
        }
    }

    //set state from saved state
    public void LoadState(dynamic data)
    {
        
    }

    public override void Interact(GameObject go)
    {
        base.Interact(go);
        
        //Debug.Log("Cutting down " + transform.name + " with health: " + health);
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
            //drop fence
            Instantiate(fence,transform.position, Quaternion.identity);
            
            //remove from objectManager list
            FenceData fd = new FenceData(this);
            //Debug.Log("Remove fence");
            objectManager.RemoveObject(this.positionMinusOffsetX, this.positionMinusOffsetY);
            //treeController.RemoveTree(td.position);
            //remove any connections
            checkConnectionsRemove();
            Destroy(gameObject);
            return;
        }
    }

    public int GetFenceType(){

        float offsetX = 0f;
        float offsetY = 0.09f;

        float sizeX = 0.25f;
        float sizeY = 0.17f;

        int f = 0;

        if((!up)&&(!down)&&(!left)&&(!right))
        {
            f = 0;
        }else if((!up)&&(!down)&&(left)&&(!right))
        {
            sizeX = 1f;
            offsetX = -0.37f;
            f = 3;
        }
        else if((!up)&&(!down)&&(!left)&&(right))
        {
            sizeX = 1f;
            offsetX = 0.37f;
            f = 1;
        }
        else if((!up)&&(!down)&&(left)&&(right))
        {
            sizeX = 1.7f;
            f = 2;
        }
        else if((up)&&(!down)&&(!left)&&(!right))
        {
            offsetY = 0.3f;
            sizeY = 1f;
            f = 5;
        }
        else if((!up)&&(down)&&(!left)&&(!right))
        {
            offsetY = -0.3f;
            sizeY = 1f;
            f = 4;
        }
        else if((up)&&(down)&&(!left)&&(!right))
        {
            sizeY = 1.7f;
            f = 6;
        }
        else if((up)&&(down)&&(left)&&(right))
        {
            f = 7;
        }
        else if((up)&&(!down)&&(left)&&(!right))
        {
            offsetY = 0.3f;
            sizeY = 1f;
            f = 8;
        }
        else if((up)&&(!down)&&(!left)&&(right))
        {
            offsetY = 0.3f;
            sizeY = 1f;
            f = 9;
        }
        else if((!up)&&(down)&&(left)&&(!right))
        {
            offsetY = -0.3f;
            sizeY = 1f;
            f = 10;
        }
        else if((!up)&&(down)&&(!left)&&(right))
        {
            offsetY = -0.3f;
            sizeY = 1f;
            f = 11;
        }
        else if((!up)&&(down)&&(left)&&(right))
        {
            offsetY = -0.3f;
            sizeY = 1f;
            f = 12;
        }
        else if((up)&&(!down)&&(left)&&(right))
        {
            offsetY = 0.3f;
            sizeY = 1f;
            f = 13;
        }
        else if((up)&&(down)&&(left)&&(!right))
        {
            f = 14;
        }
        else if((up)&&(down)&&(!left)&&(right))
        {
            f = 15;
        }

        //set collision box offset and size
        GetComponent<BoxCollider2D>().offset = new Vector2(offsetX, offsetY);
        GetComponent<BoxCollider2D>().size = new Vector2(sizeX, sizeY);

        return f;
    }

    public void CalcSprite()
    {
        Sprite[] allFences = Resources.LoadAll<Sprite>("Sprites/Placeable/FenceWood");

        //return allFences.Single(s => s.name == sprt);
        GetComponent<SpriteRenderer>().sprite = allFences[GetFenceType()];
        //return allFences[f];
    }

    public void CalcCollision(){
        
    }
    public void UpdateConnections()
    {
        //update sprite
        //GetComponent<SpriteRenderer>().sprite = CalcSprite();
        CalcSprite();
        //update box collider 2D (done in GetFenceType())
    }
    
}
