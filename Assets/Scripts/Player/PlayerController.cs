using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;//speed of player
    private int attack = 1;//player attack damage
    private int defaultAttack = 1;
    private int health = 10;//player health
    private int maxHealth = 20;
    private int hunger = 10;//player hunger
    private int maxHunger = 20;
    private int thurst = 10;//player thurst
    private int maxThurst = 20;
    private Vector2 spawn;

    private bool allowedToRun = true;
    private bool isRunning = false;
    private float effectsTimer = 3.0f;
    private float healthTimer = 3.0f;

    Animator animator;
    //hiding inherited rigidbody2D fix?
    Rigidbody2D rb2D;
    Vector2 lookDirection = new Vector2(1,0);//direction player is looking
    Vector2 move = new Vector2(1,0);
    private UIInventory uiInventory;
    private GameItem heldItem;
    private ObjectManager objectManager;
    private GameManager gameManager;
    private GameObject highlight;
    private PlayerHTH playerHTH;

    public Sprite greenHighlight;
    public Sprite redHighlight;

    private static bool allowedToMove = true;

    //instance for dont destroy
    //private static PlayerController playerInstance;

    void Awake()
    {
        uiInventory = GameObject.Find("Inventory").GetComponent<UIInventory>();
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        highlight = GameObject.Find("Highlight");
        playerHTH = GameObject.Find("PlayerHTH").GetComponent<PlayerHTH>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        highlight.SetActive(false);
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        spawn = new Vector2(0,0);

        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;

        // //keep between scenes
        // if(playerInstance == null)
        // {
        //     DontDestroyOnLoad(gameObject);
        //     playerInstance = this;
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    void Start()
    {
        playerHTH.UpdateAll(this.health, this.thurst, this.hunger);
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void IsRunning(bool tof)
    {
        this.isRunning = tof;
        if(tof)
        {
            //make player run
            SetSpeed(5);
        }
        else
        {
            //make player walk
            SetSpeed(3);
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public int GetHealth()
    {
        return this.health;
    }
    public int GetThurst()
    {
        return this.thurst;
    }
    public int GetHunger()
    {
        return this.hunger;
    }

    //kill player
    public void KillPlayer()
    {
        //set players position to spawn 
        this.transform.position = spawn;
        //reset hunger, thurst and health
        ResetHTH();
    }

    //add health to player
    public void IncrementHealth(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(this.health + 1 <= this.maxHealth)
            {
                this.health++;
                SetAllowedToRun();
            }
            else
            {
                //health full
                break;
            }
        }
        playerHTH.UpdateHealth(this.health);
    }

    //remove health from player
    public void DecrementHealth(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(this.health > 0)
            {
                this.health--;
                SetAllowedToRun();
            }
            else
            {
                //health empty (player dies)
                this.health = 0;
                //Kill player
                KillPlayer();
                break;
            }
        }
        playerHTH.UpdateHealth(this.health);
    }

    //add thurst to player
    public void IncrementThurst(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(this.thurst + 1 <= this.maxThurst)
            {
                this.thurst++;
                SetAllowedToRun();
            }
            else
            {
                //thurst full
                break;
            }
        }
        playerHTH.UpdateThurst(this.thurst);
    }

    //remove thurst from player
    public void DecrementThurst(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(this.thurst > 0)
            {
                this.thurst--;
                SetAllowedToRun();
            }
            else
            {
                //thurst empty (player takes damage)
                this.thurst = 0;
                break;
            }
        }
        playerHTH.UpdateThurst(this.thurst);
    }

    //add hunger to player
    public void IncrementHunger(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(this.hunger + 1 <= this.maxHunger)
            {
                this.hunger++;
                SetAllowedToRun();
            }
            else
            {
                //hunger full
                break;
            }
        }
        playerHTH.UpdateHunger(this.hunger);
    }

    //remove hunger from player
    public void DecrementHunger(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(this.hunger > 0)
            {
                this.hunger--;
                SetAllowedToRun();
            }
            else
            {
                //hunger empty (player takes damage)
                this.hunger = 0;
                //playerHTH.UpdateHunger(this.hunger);
                break;
            }
        }
        playerHTH.UpdateHunger(this.hunger);
    }

    public void ResetHTH()
    {
        this.health = maxHealth/2;
        this.hunger = maxHunger/2;
        this.thurst = maxThurst/2;

        playerHTH.UpdateAll(this.health, this.thurst, this.hunger);
    }

    public static void SetAllowedToMove(bool tof)
    {
        allowedToMove = tof;
    }

    public GameItem GetHeldItem()
    {
        return this.heldItem;
    }

    public int GetAttack()
    {
        return this.attack;
    }

    public int GetDefaultAttack()
    {
        return this.defaultAttack;
    }

    void Save()
    {
        PlayerData pd = new PlayerData(this);
        // SaveSystem.Save<float[]>(pd.position, "player");
        SaveSystem.Save<dynamic>(pd, "player");
        Debug.Log("Saved Player");
    }

    void Load()
    {
        if(SaveSystem.SaveExists("player"))
        {
            Debug.Log("Player Save Exists");
            //float[] data = SaveSystem.Load<float[]>("player");
            //load and set player position
            // Vector3 position;
            // position.x = data[0];
            // position.y = data[1];
            // position.z = data[2];
            // this.transform.position = position;

            PlayerData data = SaveSystem.Load<dynamic>("player");

            //load player position
            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            this.transform.position = position;

            //make player look down
            lookDirection.Set(0, -1);
            lookDirection.Normalize();

            //load player health, thurst, hunger
            //print(data.hunger);
            this.health = data.health;
            this.thurst = data.thurst;
            this.hunger = data.hunger;
            playerHTH.UpdateAll(this.hunger, this.thurst, this.health);
        }
        else
        {
            Debug.Log("Player Save Doesn't Exist");
        }
    }

    //check if hunger should be decremented when running
    public void RunningEffects()
    {
        this.effectsTimer -= Time.deltaTime;
        if(this.effectsTimer <= 0.0f)
        {
            DecrementHunger(3);
            DecrementThurst(1);
            //reset timer
            this.effectsTimer = 3.0f;
        }
    }

    //deplete health if hunger and thurst empty
    public void DamageOverTime()
    {
        this.healthTimer -= Time.deltaTime;
        if(this.healthTimer <= 0.0f)
        {
            //damage player
            DecrementHealth(2);
            //reset timer
            this.healthTimer = 3.0f;
        }
    }

    //restore health if hunger and thurst are full
    public void RecoverOverTime()
    {
        this.healthTimer -= Time.deltaTime;
        if(this.healthTimer <= 0.0f)
        {
            //restore player health
            IncrementHealth(2);
            //reset timer
            this.healthTimer = 3.0f;
        }
    }

    //check if player is allowed to run
    public void SetAllowedToRun()
    {
        if((this.hunger < this.maxHunger/4)||(this.thurst < this.maxThurst/4)||(this.health < this.maxHealth/4))
        {
            this.allowedToRun = false;
        }
        else{
            this.allowedToRun = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //see if the player should be running
        if(Input.GetKey(KeyCode.LeftControl))
        {
            //check if player allowed to run
            if(this.allowedToRun)
            {
                //increase player speed
                IsRunning(true);
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            //normal player speed
            IsRunning(false);
        }

        //check if should stop player from running
        if(!this.allowedToRun)
        {
            IsRunning(false);
        }

        //check if should decrement hunger
        if(this.isRunning)
        {
            RunningEffects();
        }

        //check if hunger and thurst depleted (if so damage player)
        //check if hunger and thurst are full (if so recover player health)
        if((this.hunger == 0)&&(this.thurst == 0))
        {
            DamageOverTime();
        }else if((this.hunger == this.maxHunger)&&(this.thurst == this.maxThurst)&&(this.health != this.maxHealth))
        {
            RecoverOverTime();
        }

        //movement

        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");

        // move = new Vector2(horizontal, vertical);
        // move.Normalize();//stop increased speed diagonally

        // if((!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))&&(allowedToMove))//if moving
        // {
        //     lookDirection.Set(move.x, move.y);
        //     lookDirection.Normalize();
        // }
        // if(allowedToMove)
        // {
        //     animator.SetFloat("Horizontal", lookDirection.x);
        //     animator.SetFloat("Vertical", lookDirection.y);
        //     animator.SetFloat("Speed", move.magnitude);
        // }

        //interact with objects through raycast
        if((Input.GetKeyDown(KeyCode.Space)))
        {
            //RaycastHit2D hit = Physics2D.CircleCast(rb2D.position + Vector2.up * 0.2f, 0.2f, lookDirection, 0.5f, LayerMask.GetMask("InteractiveObjects"));
            RaycastHit2D[] hits = Physics2D.CircleCastAll(rb2D.position + Vector2.up * 0.2f, 0.2f, lookDirection, 0.5f, LayerMask.GetMask("InteractiveObjects"));
            foreach (var hit in hits)
            {
                if(hit.collider != null)
                {
                    //check not edge collider as is used for alpha change on objects
                    if(hit.collider.GetType() != typeof(EdgeCollider2D))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if(interactable != null)
                        {
                            interactable.Interact(this.gameObject);
                        }
                        break;
                    }
                }
            }
        }
        if((Input.GetMouseButtonDown(1))&&(this.heldItem != null)&&(allowedToMove)&&(!gameManager.IsMouseOverUI()))
        {
            //check no itemSelected in UI ------------------------------------------------------------------------------
            if(this.heldItem.placeable)
            {
                //place object
                GameObject itemToPlace = Resources.Load<GameObject>("Placeable/" + this.heldItem.title);
                //position to place object (depending on look direction (infornt of plater))

                // float x = (float)Math.Round(this.transform.position.x + PlaceOffset().x) + 0.5f;
                // float y = (float)Math.Round(this.transform.position.y + PlaceOffset().y) + 0.5f;
                float x = GetPosInfrontOfPlayer().x;
                float y = GetPosInfrontOfPlayer().y;
                //check space is free
                if(objectManager.IsSpaceFree(x,y))
                {
                    Vector3 itemToPlacePos = new Vector3(x,y,0);
                    Instantiate(itemToPlace, itemToPlacePos, Quaternion.identity);
                    //objectManager.AddObject(x,y,this.heldItem.title, itemToPlace);
                    //remove item from inventory
                    //GameInventory.instance.RemoveAmountOfItem(this.heldItem.title, 1);
                    if(this.heldItem.count < 2)
                    {
                        GameInventory.instance.RemoveItem(this.heldItem);
                    }
                    else if(this.heldItem.count > 1)
                    {
                        //decrement count of item by 1
                        GameInventory.instance.RemoveAmountOfItem(this.heldItem, 1);
                    }
                }
            }
        }
        // if(uiInventory.GetSelectedItem() != null)
        // {
        //     this.heldItem = uiInventory.GetSelectedItem();
        // }
        
        this.heldItem = uiInventory.GetSelectedItem();
        
        UpdateStats();
        if(this.heldItem != null)
        {
            //check if held item is placeable
            if(this.heldItem.placeable)
            {
                //set highlight position
                highlight.transform.position = new Vector3((float)Math.Round(this.transform.position.x + PlaceOffset().x) + 0.5f,(float)Math.Round(this.transform.position.y + PlaceOffset().y) + 0.5f,0);
                
                //check if highlight position is free
                if(objectManager.IsSpaceFree((float)Math.Round(this.transform.position.x + PlaceOffset().x) + 0.5f,(float)Math.Round(this.transform.position.y + PlaceOffset().y) + 0.5f))
                {
                    //space free (green)
                    highlight.GetComponent<SpriteRenderer>().sprite = greenHighlight;
                }
                else{
                    //space not free (red)
                    highlight.GetComponent<SpriteRenderer>().sprite = redHighlight;
                }

                //set highlight visible
                highlight.SetActive(true);
            }else{
                //set highlight invisible
                highlight.SetActive(false);
            }
        }else{
            highlight.SetActive(false);
        }
    }

    //get vector3 of tile infrom of player
    public Vector3 GetPosInfrontOfPlayer()
    {
        Vector3 pos = new Vector3((float)Math.Round(this.transform.position.x + PlaceOffset().x) + 0.5f, (float)Math.Round(this.transform.position.y + PlaceOffset().y) + 0.5f, 0);
        return pos;
    } 

    //calculate placement offset depending on look direction
    public Vector2 PlaceOffset()
    {
        //default to right
        Vector2 placeOffset = new Vector2(0.5f, -0.5f);

        if((lookDirection.x == 0)&&(lookDirection.y == 1))
        {
            //up
            placeOffset = new Vector2(-0.5f, 0.5f);
        }else if((lookDirection.x == 0)&&(lookDirection.y == -1))
        {
            //down
            placeOffset = new Vector2(-0.5f, -1.5f);
        }else if((lookDirection.x == -1)&&(lookDirection.y == 0))
        {
            //left
            placeOffset = new Vector2(-1.5f, -0.5f);
        }else if((lookDirection.x == 1)&&(lookDirection.y == 0))
        {
            //right
            placeOffset = new Vector2(0.5f, -0.5f);
        }

        return placeOffset;
    }

    //use to stop framerate issues
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        move = new Vector2(horizontal, vertical);
        move.Normalize();//stop increased speed diagonally

        if(allowedToMove)
        {
            if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))//if moving
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            animator.SetFloat("Horizontal", lookDirection.x);
            animator.SetFloat("Vertical", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);

            Vector2 position = rb2D.position;//change position
            position = position + move*speed*Time.fixedDeltaTime;
            rb2D.MovePosition(position);
        }
        else
        {
            //player speed set to 0 when not allowed to move
            animator.SetFloat("Speed", 0);
        }

        // if((!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))&&(allowedToMove))//if moving
        // {
        //     lookDirection.Set(move.x, move.y);
        //     lookDirection.Normalize();
        // }
        // if(allowedToMove)
        // {
        //     animator.SetFloat("Horizontal", lookDirection.x);
        //     animator.SetFloat("Vertical", lookDirection.y);
        //     animator.SetFloat("Speed", move.magnitude);
        // }

        // if(allowedToMove)
        // {
        //     Vector2 position = rb2D.position;//change position
        //     position = position + move*speed*Time.fixedDeltaTime;
        //     rb2D.MovePosition(position);
        // }
    }

    void UpdateStats()
    {
        //update player stats
        if((this.heldItem != null)&&(this.heldItem.stats != null))
        {
            //attack
            if(this.heldItem.stats.ContainsKey("Attack"))
            {
                this.attack = this.heldItem.stats["Attack"];
            }
            else
            {
                this.attack = this.defaultAttack;
            }
        }
        // if((this.heldItem != null)&&(this.heldItem.placeable))
        // {
            
        // }
    }

    void OnDestroy()
    {
        GameEvents.SaveInitiated -= Save;
        GameEvents.LoadInitiated -= Load;
    }
}
