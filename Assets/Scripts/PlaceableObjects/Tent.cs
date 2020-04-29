using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player collides change scene to inside tent
        if(collision.gameObject.tag == "Player")
        {
            //set player position to entrance of tent
            
            //enter tent (change scene)
            SceneManager.LoadScene("Tent");
        }
    }
}
