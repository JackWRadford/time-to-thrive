using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class FenceController : Interactable
{

    private ObjectManager objectManager;

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;
    // Start is called before the first frame update
    void Awake()
    {

        Debug.Log("Place fence");
        //on placement raycast UDLR to check for adjacent fences
        //RaycastHit2D[] hitsU = Physics2D.RaycastAll(rb2D.position + Vector2.up * 0.2f, 0.2f, lookDirection, 0.5f, LayerMask.GetMask("InteractiveObjects"));
        Debug.Log(this.transform.position);
        RaycastHit2D[] hitsU = Physics2D.RaycastAll(this.transform.position + new Vector3(0, 0.3f, 0), new Vector2(0,1), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsU)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    Debug.Log("Fence Up");
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
                    Debug.Log("Fence Down");
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
                    Debug.Log("Fence Left");
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
                    Debug.Log("Fence Right");
                    hit.collider.GetComponent<FenceController>().left = true;
                    hit.collider.GetComponent<FenceController>().UpdateConnections();
                    this.right = true;
                }
            }
        }
        UpdateConnections();

        // objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        // UpdateState();
    }

    void Start()
    {
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        UpdateState();
    }

    //update state to be saved
    public void UpdateState()
    {
        FenceData fd = new FenceData(this);
        //check object not already in that position (double save in same position)
        if(objectManager.IsSpaceFree(this.transform.position.x,this.transform.position.y))
        {
            Debug.Log("Add fence");
            objectManager.AddObject(this.transform.position.x, this.transform.position.y, "Fence", fd);
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
