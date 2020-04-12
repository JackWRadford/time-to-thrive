using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;//speed of player
    private int attack = 1;//player attack damage
    private int defaultAttack = 1;

    Animator animator;
    //hiding inherited rigidbody2D fix?
    Rigidbody2D rb2D;
    Vector2 lookDirection = new Vector2(1,0);//direction player is looking
    Vector2 move = new Vector2(1,0);
    private UIInventory uiInventory;
    private GameItem heldItem;

    private static bool allowedToMove = true;

    void Awake()
    {
        uiInventory = GameObject.Find("Inventory").GetComponent<UIInventory>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;
    }

    void Start()
    {
        

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
        SaveSystem.Save<float[]>(pd.position, "player");
    }

    void Load()
    {
        if(SaveSystem.SaveExists("player"))
        {
            float[] data = SaveSystem.Load<float[]>("player");
            //load and set player position
            Vector3 position;
            position.x = data[0];
            position.y = data[1];
            position.z = data[2];
            this.transform.position = position;

            //make player look down
            lookDirection.Set(0, -1);
            lookDirection.Normalize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);
        move.Normalize();//stop increased speed diagonally

        if((!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))&&(allowedToMove))//if moving
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        if(allowedToMove)
        {
            animator.SetFloat("Horizontal", lookDirection.x);
            animator.SetFloat("Vertical", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }
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
        if((Input.GetMouseButtonDown(1))&&(this.heldItem.placeable))
        {
            GameObject itemToPlace = Resources.Load<GameObject>("Placeable/" + this.heldItem.title);
            Vector3 itemToPlacePos = new Vector3((float)Math.Round(this.transform.position.x + 0.5f),(float)Math.Round(this.transform.position.y + 0.5f),0);
            Instantiate(itemToPlace, itemToPlacePos, Quaternion.identity);
        }
        this.heldItem = uiInventory.GetSelectedItem();
        UpdateStats();
        // if(uiInventory.GetSelectedItem() != null)
        // {
        //     Debug.Log("holding: " + heldItem.title);
        // }
    }
    //use to stop framerate issues
    void FixedUpdate()
    {
        if(allowedToMove)
        {
            Vector2 position = rb2D.position;//change position
            position = position + move*speed*Time.fixedDeltaTime;
            rb2D.MovePosition(position);
        }
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
