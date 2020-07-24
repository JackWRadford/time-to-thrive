using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //make gameManager aware that mouse is over UIItem 
        gameManager.mouseOverBagUI = true;
        //Debug.Log("in");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //make gameManager aware that mouse is not over UIItem 
        gameManager.mouseOverBagUI = false;
        //Debug.Log("out");
    }
}
