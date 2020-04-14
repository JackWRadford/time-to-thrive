using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Place fence");
        //on placement raycast UDLR to check for adjacent fences
        //RaycastHit2D[] hitsU = Physics2D.RaycastAll(rb2D.position + Vector2.up * 0.2f, 0.2f, lookDirection, 0.5f, LayerMask.GetMask("InteractiveObjects"));
        Debug.Log(this.transform.position);
        RaycastHit2D[] hitsU = Physics2D.RaycastAll(this.transform.position + new Vector3(0, 0.3f, 0), new Vector2(0,1), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsU)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    Debug.Log("Fence Up");
                }
            }
        }
        RaycastHit2D[] hitsD = Physics2D.RaycastAll(this.transform.position + new Vector3(0, -0.3f, 0), new Vector2(0,-1), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsD)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    Debug.Log("Fence Down");
                }
            }
        }
        RaycastHit2D[] hitsL = Physics2D.RaycastAll(this.transform.position + new Vector3(-0.3f, 0.1f, 0), new Vector2(-1,0), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsL)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    Debug.Log("Fence Left");
                }
            }
        }
        RaycastHit2D[] hitsR = Physics2D.RaycastAll(this.transform.position + new Vector3(0.3f, 0.1f, 0), new Vector2(1,0), 1f, LayerMask.GetMask("InteractiveObjects"));
        foreach (var hit in hitsR)
        {
            if(hit.collider != null)
            { 
                if(hit.collider.GetComponent<FenceController>())
                {
                    Debug.Log("Fence Right");
                }
            }
        }
    }

}
