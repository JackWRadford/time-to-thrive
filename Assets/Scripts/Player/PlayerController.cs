using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;//speed of player

    Animator animator;
    //hiding inherited rigidbody2D fix?
    Rigidbody2D rb2D;
    Vector2 lookDirection = new Vector2(1,0);//direction player is looking
    Vector2 move = new Vector2(1,0);

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        //listen for save event
        // GameEvents.SaveInitiated += Save;

    }

    // void Save()
    // {
    //     SaveSystem.Save<Vector3>(transform.position, "playerposition");
    // }

    // void Load()
    // {
    //     if(SaveSystem.SaveExists("playerposition"))
    //     {
    //         transform.position = SaveSystem.Load<Vector3>("playerposition");
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);
        move.Normalize();//stop increased speed diagonally

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))//if moving
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //interact with objects through raycast
        if((Input.GetKeyDown(KeyCode.Space)))
        {
            RaycastHit2D hit = Physics2D.CircleCast(rb2D.position + Vector2.up * 0.2f, 0.2f, lookDirection, 0.5f, LayerMask.GetMask("InteractiveObjects"));
            if(hit.collider != null)
            {
                //check not edge collider as is used for alpha change on objects
                if(hit.collider.GetType() != typeof(EdgeCollider2D))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null)
                {
                    interactable.Interact();
                }
                }
            }
        }
    }
    //use to stop framerate issues
    void FixedUpdate()
    {
        Vector2 position = rb2D.position;//change position
        position = position + move*speed*Time.fixedDeltaTime;
        rb2D.MovePosition(position);
    }

    
}
