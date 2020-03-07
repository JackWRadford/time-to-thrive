using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree : MonoBehaviour
{
    EdgeCollider2D edgeCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
