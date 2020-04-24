using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
