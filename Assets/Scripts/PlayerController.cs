using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //speed of player
    public float speed = 3.0f;

    //player animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    Vector2 move = new Vector2(1,0);

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position = position + move*speed*Time.fixedDeltaTime;
        transform.position = position;
    }
}
