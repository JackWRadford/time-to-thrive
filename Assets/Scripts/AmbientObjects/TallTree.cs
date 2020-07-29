using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TallTree : Interactable, ILoadState
{
    EdgeCollider2D edgeCollider2D;
    public TreeController treeController;
    public ObjectManager objectManager;
    public PlacementOffset placementOffset;
    public GameObject stick;
    public GameObject apple;
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
        //SaveState();

        this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
        this.positionMinusOffsetY = this.transform.position.y - placementOffset.GetOffsetY();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //update state to be saved
    // public void SaveState()
    // {
    //     this.positionMinusOffsetX = this.transform.position.x - placementOffset.GetOffsetX();
    //     this.positionMinusOffsetY = this.transform.position.y- placementOffset.GetOffsetY();

    //     TreeData td = new TreeData(this);
    //     //check object not already in that position (double save in same position)
    //     //if(objectManager.IsSpaceFree(this.positionMinusOffsetX,this.positionMinusOffsetY))
    //     //{
    //         objectManager.AddObject(this.positionMinusOffsetX, this.positionMinusOffsetY, "Tree", td);
    //     //}
    // }
    
    //set state from saved state
    public void LoadState(dynamic data)
    {
        //this.health = data.health;
    }

    public void UpdateState()
    {
        //find and remove data from objects list
        objectManager.RemoveObject(this.positionMinusOffsetX + 0.5f,this.positionMinusOffsetY + 0.5f);
        //add updated data to objects list
        TreeData td = new TreeData(GetComponent<TallTree>());
        objectManager.AddObject(this.positionMinusOffsetX + 0.5f,this.positionMinusOffsetY + 0.5f,"Tree",td);
    }

    public int GetHealth()
    {
        return this.health;
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
        //drop logs
        int randNumLogs = Random.Range(2,4);
        for(int i = 0; i < randNumLogs; i++ )
        {
            Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
            Instantiate(log,transform.position + randDist, Quaternion.identity);
        }
        //drop sticks
        int randNumSticks = Random.Range(1,6);
        for(int i = 0; i < randNumSticks; i++ )
        {
            Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
            Instantiate(stick,transform.position + randDist, Quaternion.identity);
        }
        //drop apples
        int randNumApples = Random.Range(0,3);
        for(int i = 0; i < randNumApples; i++ )
        {
            Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
            Instantiate(apple,transform.position + randDist, Quaternion.identity);
        }
        //remove from treeController list of tree positions
        TreeData td = new TreeData(this);
        //Debug.Log("Remove tree");
        objectManager.RemoveObject(this.positionMinusOffsetX + 0.5f,this.positionMinusOffsetY + 0.5f);
        //treeController.RemoveTree(td.position);
        Destroy(gameObject);
        return;
        }
        //update state to be saved
        //UpdateState();
    }

    //check if player is behind tree
    private void OnTriggerStay2D(Collider2D other) {
        //make tree transparent
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.6f;
        transform.GetComponent<SpriteRenderer>().color = tmp;
    }

    //check if player exists from behind tree
    private void OnTriggerExit2D(Collider2D other) {
        //make tree sprite alpha 1.0f
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 1.0f;
        transform.GetComponent<SpriteRenderer>().color = tmp;
    }
}
