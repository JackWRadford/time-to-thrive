using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TallTree : Interactable
{
    EdgeCollider2D edgeCollider2D;
    public TreeController treeController;
    public GameObject stick;
    public GameObject apple;
    public GameObject log;

    int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        treeController = GameObject.Find("GameManager").GetComponent<TreeController>();
    }

    // Update is called once per frame
    void Update()
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
        treeController.RemoveTree(td.position);
        Destroy(gameObject);
        }
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
