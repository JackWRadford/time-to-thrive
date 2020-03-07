using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;//speed of player

    Animator animator;
    //hiding inherited rigidbody2D fix?
    Rigidbody2D rb2D;
    Vector2 lookDirection = new Vector2(1,0);//direction player is moving
    Vector2 move = new Vector2(1,0);

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

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
    }
    //use to stop framerate issues
    void FixedUpdate()
    {
        Vector2 position = rb2D.position;//change position
        position = position + move*speed*Time.fixedDeltaTime;
        rb2D.MovePosition(position);
    }
}
