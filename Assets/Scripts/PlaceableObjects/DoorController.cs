using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
CLass for Doors to check for valid structure
*/
public class DoorController : Interactable
{
    //private bool structureExists = false;

    private bool doorOpen = false;
    public int orientation = 0;
    public GameObject alternativeDoor = null;
    public Vector2 offsetOne = new Vector2(0f,0.1f);
    public Vector2 sizeOne = new Vector2(1f,0.18f);
    public Vector2 offsetTwo = new Vector2(0f,0.5f);
    public Vector2 sizeTwo = new Vector2(0.17f,1f);

    public override void NDInteract(GameObject go)
    {
        base.NDInteract(go);
        Debug.Log("open door");

        //if door is open close it vice-versa
        OpenCloseDoor();
    }

    public void OpenCloseDoor()
    {
        if(this.doorOpen)
        {
            //close
            switch(this.orientation)
            {
                case 0:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetOne;
                    this.GetComponent<BoxCollider2D>().size = this.sizeOne;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[0];
                    //change position to account for offset of new sprite
                    Vector3 newPos = this.transform.position + new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) - new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos;
                    break;
                case 1:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetTwo;
                    this.GetComponent<BoxCollider2D>().size = this.sizeTwo;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[1];
                    //change position to account for offset of new sprite
                    Vector3 newPos1 = this.transform.position + new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) - new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos1;
                    break;
                case 2:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetOne;
                    this.GetComponent<BoxCollider2D>().size = this.sizeOne;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[2];
                    //change position to account for offset of new sprite
                    Vector3 newPos2 = this.transform.position + new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) - new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos2;
                    break;
                case 3:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetTwo;
                    this.GetComponent<BoxCollider2D>().size = this.sizeTwo;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[3];
                    //change position to account for offset of new sprite
                    Vector3 newPos3 = this.transform.position + new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) - new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos3;
                    break;
            }
            this.doorOpen = false;
        }
        else
        {
            //open
            switch(this.orientation)
            {
                case 0:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetTwo;
                    this.GetComponent<BoxCollider2D>().size = this.sizeTwo;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[1];
                    //change position to account for offset of new sprite
                    Vector3 newPos = this.transform.position - new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) + new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos;
                    break;
                case 1:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetOne;
                    this.GetComponent<BoxCollider2D>().size = this.sizeOne;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[0];
                    //change position to account for offset of new sprite
                    Vector3 newPos1 = this.transform.position - new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) + new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos1;
                    break;
                case 2:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetTwo;
                    this.GetComponent<BoxCollider2D>().size = this.sizeTwo;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[1];
                    //change position to account for offset of new sprite
                    Vector3 newPos2 = this.transform.position - new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) + new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos2;
                    break;
                case 3:
                    //set box collider
                    this.GetComponent<BoxCollider2D>().offset = this.offsetOne;
                    this.GetComponent<BoxCollider2D>().size = this.sizeOne;
                    //set sprite
                    this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/Placeable/Door")[0];
                    //change position to account for offset of new sprite
                    Vector3 newPos3 = this.transform.position - new Vector3(this.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.GetComponent<PlacementOffset>().GetOffsetY(),0) + new Vector3(this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetX(),
                    this.alternativeDoor.GetComponent<PlacementOffset>().GetOffsetY(),0);
                    this.transform.position = newPos3;
                    break;
            }


            this.doorOpen = true;
        }
    }



}
