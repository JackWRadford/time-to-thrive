using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSapling : Interactable, ILoadState
{
    public ObjectManager objectManager;
    public PlacementOffset placementOffset;
    public StackDetails stackDetails;
    public GameObject sapling;
    public GameObject tree;

    private int health = 3;

    private float positionMinusOffsetX;
    private float positionMinusOffsetY;

    private float growth = 0f;
    public float growthThreshold = 5f;
    private float timer = 0f;
    public float delay = 1f;

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

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > delay)
        {
            //increase growth
            timer = 0f;
            IterateGrowth();
        }
    }

    public float GetGrowth()
    {
        return this.growth;
    }

    public void SaveState()
    {
        SaplingData sd = new SaplingData(this);
        //check object not already in that position (double save in same position)
        if(objectManager.IsSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY, "OakTreeSapling", sd);
        }
    }

    //save GO to list (data already saved and loaded)
    public void SaveGO()
    {
        //check object not already in that position (double save in same position)
        if(objectManager.IsGOSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY, this.gameObject))
        {
            //Debug.Log("add GO" + this.positionMinusOffsetX + 0.5f.ToString() + this.positionMinusOffsetY + 0.5f.ToString());
            objectManager.AddObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY, "OakTreeSapling", this.gameObject);
        }
    }
    
    //set state from saved state
    public void LoadState(dynamic data)
    {
        // Debug.Log("load growth state: " + data.growth);
        // //load sapling growth
        // this.growth = data.growth;
    }

    // public void UpdateState()
    // {
    //     //find and remove data from objects list
    //     objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
    //     //add updated data to objects list
    //     SaplingData sd = new SaplingData(this);
    //     objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY,"OakTreeSapling",sd);
    // }

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
        //drop sapling
        Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
        Instantiate(sapling,transform.position + randDist, Quaternion.identity);
        
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

    public void IterateGrowth()
    {
        this.growth++;
        if(this.growth >= this.growthThreshold)
        {
            //remove sapling and replace with tree
            this.growth = 0;

            //remove the sapling from objects and data Lists
            objectManager.RemoveObject(this.positionMinusOffsetX,this.positionMinusOffsetY);
            objectManager.RemoveObjectGO(this.positionMinusOffsetX, this.positionMinusOffsetY);
            //instantiate Tree at this sapling position and save to objects and data Lists
            GameObject newTree = Instantiate(tree, this.transform.position, Quaternion.identity) as GameObject;
            newTree.name = tree.name;
            TreeData td = new TreeData(newTree.GetComponent<TallTree>());
            objectManager.AddObjectData(this.positionMinusOffsetX,this.positionMinusOffsetY, "OakTreeStump", td);
            objectManager.AddObjectGO(this.positionMinusOffsetX,this.positionMinusOffsetY, "OakTreeStump", newTree);

            Destroy(gameObject);
            return;
        }
    }
}
