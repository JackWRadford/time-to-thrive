using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cow : Interactable, ILoadState
{
    public ObjectManager objectManager;
    Rigidbody2D rb2D;
    Vector2 lookDirection = new Vector2(1,0);//direction player is looking
    Vector2 move = new Vector2(0,0);
    private int health = 10;
    private float speed = 0.7f;
    private int timer = 0;
    private Vector2 lastPosition;

    public GameObject rawBeef;
    public GameObject leather;

    void Awake()
    {
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        // animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        GameEvents.PreSaveInitiated += Save;
    }

    void Start()
    {
        lastPosition = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //get horizontal  and vertical movement
        GenerateMovement();
        move.Normalize();//stop increased speed diagonally

        if((!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)))//if moving
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        // animator.SetFloat("Horizontal", lookDirection.x);
        // animator.SetFloat("Vertical", lookDirection.y);
        // animator.SetFloat("Speed", move.magnitude);
    }

    //use to stop framerate issues
    void FixedUpdate()
    {  
        Vector2 position = rb2D.position;//change position
        position = position + move*speed*Time.fixedDeltaTime;
        rb2D.MovePosition(position);
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
                if(go.GetComponent<PlayerController>().GetHeldItem().stats.ContainsKey("Attack"))
                {
                    health-=go.GetComponent<PlayerController>().GetHeldItem().stats["Attack"];
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
            //drop rawbeef
            int randNumRawBeef = Random.Range(1,3);
            for(int i = 0; i < randNumRawBeef; i++ )
            {
                Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
                Instantiate(rawBeef,transform.position + randDist, Quaternion.identity);
            }
            //drop leather
            int randNumLeather = Random.Range(0,2);
            for(int i = 0; i < randNumLeather; i++ )
            {
                Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
                Instantiate(leather,transform.position + randDist, Quaternion.identity);
            }
            
            //remove from objectManager list
            objectManager.RemoveObject(lastPosition.x, lastPosition.y);
            Destroy(gameObject);
            return;
        }
    }

    public void GenerateMovement()
    {

        if(timer <= 0)
        {
            timer = Random.Range(100, 500);
            //new random direction
            int randX = Random.Range(-1,2);
            int randY = Random.Range(-1,2);
            move = new Vector2(randX,randY);
            //Debug.Log(randX + " " + randY);
            //Debug.Log(timer);
        }
        else
        {
            timer--;
            //move in same direction
        }
    }

    //set state from saved state
    public void LoadState(dynamic data)
    {
        this.health = data.health;
    }

    public void UpdateState()
    {
        //find and remove data from objects list (use position last saved)
        objectManager.RemoveObject(this.lastPosition.x,this.lastPosition.y);
        //add updated data to objects list
        CowData cd = new CowData(this.GetComponent<Cow>());
        objectManager.AddObject(this.transform.position.x,this.transform.position.y,"Cow",cd);
        this.lastPosition = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    void Save()
    {
        UpdateState();
    }

    void OnDestroy()
    {
        GameEvents.PreSaveInitiated -= Save;
        //GameEvents.LoadInitiated -= Load;
    }
}
