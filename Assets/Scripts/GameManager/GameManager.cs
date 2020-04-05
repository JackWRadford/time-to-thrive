using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject craftingUI;
    public GameObject bagUI;
    public GameObject pauseMenuUI;
    public GameObject inventory;
    public GameObject canvas;

    void Start()
    {
        Load();
    }
    void Update()
    {   
        //delete all save files
        // if(Input.GetKeyDown(KeyCode.P))
        // {
        //     SaveSystem.DeleteAllSaveFiles();
        // }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if((!craftingUI.activeInHierarchy)&&(!bagUI.activeInHierarchy))
            {
                if(!pauseMenuUI.activeInHierarchy)
                {
                    //pause game
                    GetComponent<PauseMenu>().Pause();
                }
                else
                {
                    //resume game
                    GetComponent<PauseMenu>().Resume();
                }
            }
            if((!pauseMenuUI.activeInHierarchy)&&(bagUI.activeInHierarchy))
            {
                //close bag
                inventory.GetComponent<UIInventory>().CloseBag();
                if(!craftingUI.activeInHierarchy)
                {
                    PlayerController.SetAllowedToMove(true);
                }
            }
            if((!pauseMenuUI.activeInHierarchy)&&(craftingUI.activeInHierarchy))
            {
                //close crafting
                canvas.GetComponent<UICrafting>().CloseCrafting();
                if(!bagUI.activeInHierarchy)
                {
                    PlayerController.SetAllowedToMove(true);
                }
            }
        }

        if((Input.GetKeyDown(KeyCode.E))&&(!pauseMenuUI.activeInHierarchy))
        {
            if(!bagUI.activeInHierarchy)
            {
                //open bag
                inventory.GetComponent<UIInventory>().OpenBag();
                PlayerController.SetAllowedToMove(false);
            }
            else
            {
                //close bag
                inventory.GetComponent<UIInventory>().CloseBag();
                if(!craftingUI.activeInHierarchy)
                {
                    PlayerController.SetAllowedToMove(true);
                }
            }
        }

        if((Input.GetKeyDown(KeyCode.Q))&&(!pauseMenuUI.activeInHierarchy))
        {
            if(!craftingUI.activeInHierarchy)
            {
                //open crafting
                canvas.GetComponent<UICrafting>().OpenCrafting();
                PlayerController.SetAllowedToMove(false);
            }
            else
            {
                //close crafting
                canvas.GetComponent<UICrafting>().CloseCrafting();
                if(!bagUI.activeInHierarchy)
                {
                    PlayerController.SetAllowedToMove(true);
                }
            }
        }
    }

    public void Save()
    {
        GameEvents.OnSaveInitiated();
    }

    public void Load()
    {
        GameEvents.OnLoadInitiated();
    }
}
