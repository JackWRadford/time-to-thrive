using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallTree : Interactable
{
    EdgeCollider2D edgeCollider2D;
    public GameObject stick;
    public GameObject apple;

    int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();
        
        //Debug.Log("Cutting down " + transform.name + " with health: " + health);
        health-=5;
        if(health<=0)
        {
        //drop stick at tree position
        int randNumSticks = Random.Range(1,6);
        for(int i = 0; i < randNumSticks; i++ )
        {
            Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
            Instantiate(stick,transform.position + randDist, Quaternion.identity);
        }
        int randNumApples = Random.Range(0,3);
        for(int i = 0; i < randNumApples; i++ )
        {
            Vector3 randDist = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0);
            Instantiate(apple,transform.position + randDist, Quaternion.identity);
        }
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
