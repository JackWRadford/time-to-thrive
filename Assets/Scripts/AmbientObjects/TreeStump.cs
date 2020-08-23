using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeStump : Interactable, ILoadState
{
    EdgeCollider2D edgeCollider2D;
    public TreeController treeController;
    public ObjectManager objectManager;
    public PlacementOffset placementOffset;
    public GameObject log;

    private int health = 1;

    private float positionMinusOffsetX;
    private float positionMinusOffsetY;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        treeController = GameObject.Find("GameManager").GetComponent<TreeController>();
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        placementOffset = this.gameObject.GetComponent<PlacementOffset>();

        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y - placementOffset.GetOffsetY();

        SaveGO();
    }

    //save GO to list (data already saved and loaded)
    public void SaveGO()
    {
        //check object not already in that position (double save in same position)
        if(objectManager.IsGOSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            //Debug.Log("add GO" + this.positionMinusOffsetX + 0.5f.ToString() + this.positionMinusOffsetY + 0.5f.ToString());
            objectManager.AddObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY, "OakTreeStump", this.gameObject);
        }
    }
    
    //set state from saved state
    public void LoadState(dynamic data)
    {
        //this.health = data.health;
    }

    public void UpdateState()
    {
        //find and remove data from objects list
        objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
        //add updated data to objects list
        TreeData td = new TreeData(GetComponent<TallTree>());
        objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY,"OakTreeStump",td);
    }

    public int GetHealth()
    {
        return this.health;
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
            }
        }
        if(health<=0)
        {
        //drop log
        Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
        Instantiate(log,transform.position + randDist, Quaternion.identity);
        
        //remove from treeController list of tree positions
        //StumpData sd = new StumpData(this);
        //Debug.Log("Remove tree: " + this.positionMinusOffsetX.ToString() + ", " + this.positionMinusOffsetY);
        objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
        objectManager.RemoveObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY);

        //treeController.RemoveTree(td.position);
        Destroy(gameObject);
        return;
        }
        //update state to be saved
        //UpdateState();
    }
}
