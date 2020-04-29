using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player collides change scene to inside tent
        if(collision.gameObject.tag == "Player")
        {
            //set player position to entrance of tent (0, -4)
            
            //collision.gameObject.GetComponent<PlayerController>().SetPosition(entrancePos);

            //enter tent (change scene)
            SceneManager.LoadScene("MainScene");
        }
    }
}
