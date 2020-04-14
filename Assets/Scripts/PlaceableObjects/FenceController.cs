using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FenceController : MonoBehaviour
{

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;
    // Start is called before the first frame update
    void Start()
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
    }

    public Sprite CalcSprite()
    {
        string sprt = "simpleFence_0";
        int f = 0;
        if((!up)&&(!down)&&(!left)&&(!right))
        {
            //no connections
            //sprt = "simpleFence_0";
            f = 0;
        }else if((!up)&&(!down)&&(left)&&(!right))
        {
            //left connection
            //sprt = "simpleFence_3";
            f = 3;
        }
        else if((!up)&&(!down)&&(!left)&&(right))
        {
            //right connection
            //sprt = "simpleFence_1";
            f = 1;
        }
        else if((!up)&&(!down)&&(left)&&(right))
        {
            //left and right connection
            //sprt = "simpleFence_2";
            f = 2;
        }
        else if((up)&&(!down)&&(!left)&&(!right))
        {
            //up connection
            //sprt = "simpleFence_2";
            f = 5;
        }
        else if((!up)&&(down)&&(!left)&&(!right))
        {
            //down connection
            //sprt = "simpleFence_2";
            f = 4;
        }
        else if((up)&&(down)&&(!left)&&(!right))
        {
            //up and down connection
            //sprt = "simpleFence_2";
            f = 6;
        }
        Sprite[] allFences = Resources.LoadAll<Sprite>("Sprites/Placeable/FenceWood");

        //return allFences.Single(s => s.name == sprt);
        return allFences[f];
    }

    public void UpdateConnections()
    {
        //update sprite
        GetComponent<SpriteRenderer>().sprite = CalcSprite();
    }
    
}
