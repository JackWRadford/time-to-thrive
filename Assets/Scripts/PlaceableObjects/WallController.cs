using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallController : Interactable, ILoadState
{
    private ObjectManager objectManager;
    public GameObject wall;

    public int health = 1;

    void Start()
    {   
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        SaveState();
    }

    //update state to be saved
    public void SaveState()
    {
        WallData wd = new WallData(this);
        //check object not already in that position (double save in same position)
        if(objectManager.IsSpaceFree(this.transform.position.x,this.transform.position.y))
        {
            //Debug.Log("Add fence");
            objectManager.AddObject(this.transform.position.x, this.transform.position.y, "Wall", wd);
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
}
