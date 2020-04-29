using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tent : MonoBehaviour
{
    private Vector3 entrancePos = new Vector3(0,-4,0);
    //private GameObject player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player collides change scene to inside tent
        if(collision.gameObject.tag == "Player")
        {
            //set player position to entrance of tent (0, -4)
            
            collision.gameObject.GetComponent<PlayerController>().SetPosition(entrancePos);

            //enter tent (change scene)
            SceneManager.LoadScene("Tent");
        }
    }
}
