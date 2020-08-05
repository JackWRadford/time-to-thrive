using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
CLass for Doors to check for valid structure
*/
public class DoorController : NDInteractable
{
    private bool structureExists = false;

    public override void NDInteract(GameObject go)
    {
        base.NDInteract(go);

        if(this.structureExists)
        {
            //check scene to inside structure (load structure interior)
            EnterStructure();
        }
        else if(!this.structureExists)
        {
            //check if structure can be created
            if(TryToCreateStructure())
            {
                Debug.Log("successfully created structure");
                this.structureExists = true;
            }
            else
            {
                Debug.Log("structure not valid");
            }
        }
    }

    public void EnterStructure()
    {

    }

    public bool TryToCreateStructure()
    {
        

        return false;
    }
}
