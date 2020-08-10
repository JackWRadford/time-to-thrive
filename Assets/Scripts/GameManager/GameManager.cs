using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public GameObject craftingUI;
    //public GameObject bagUI;
    public GameObject pauseMenuUI;
    public GameObject mainHUD;
    public GameObject inventory;
    public GameObject canvas;
    public GameObject player;

    // public bool bagOpen = false;
    // public bool craftingOpen = false;

    public bool mainHUDopen = false;

    //attribute to state if player is hovering over UI or not
    public bool mouseOverMenuUi = false;
    public bool mouseOverHotBarUI = false;
    //public bool mouseOverBagUI = false;
    //public bool mouseOverCraftUI = false;
    public bool mouseOverMainHUDUI = false;

    //private static GameManager gameManagerInstance;

    void Awake()
    {
        Instantiate(player);

        // //keep between scenes
        
        // if(gameManagerInstance == null)
        // {
        //     DontDestroyOnLoad(gameObject);
        //     gameManagerInstance = this;
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }
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

        if(Input.GetKeyDown(KeyCode.E))
        {
            //if pause menu is not active open/close main HUD UI
            if(!this.pauseMenuUI.activeInHierarchy)
            {
                if(!mainHUD.activeInHierarchy)
                {
                    //open HUD
                    canvas.GetComponent<HUDController>().OpenMainHUD();
                    PlayerController.SetAllowedToMove(false);
                }
                else
                {
                    //close HUD
                    canvas.GetComponent<HUDController>().CloseMainHUD();
                    this.mouseOverMainHUDUI = false;
                    if(!mainHUD.activeInHierarchy)
                    {
                        PlayerController.SetAllowedToMove(true);
                    }
                }
            }
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //if main HUD is not active open/close pause menu
            if(!this.mainHUD.activeInHierarchy)
            {
                if(!pauseMenuUI.activeInHierarchy)
                {
                    //pause game
                    GetComponent<PauseMenu>().Pause();
                    this.mouseOverMenuUi = true;
                }
                else
                {
                    //resume game
                    GetComponent<PauseMenu>().Resume();
                    this.mouseOverMenuUi = false;
                }
            }

            //if main HUD is open, close it
            if(this.mainHUD.activeInHierarchy)
            {
                //close HUD
                canvas.GetComponent<HUDController>().CloseMainHUD();
                this.mouseOverMainHUDUI = false;
                if(!mainHUD.activeInHierarchy)
                {
                    PlayerController.SetAllowedToMove(true);
                }
            }

            // if((!craftingUI.activeInHierarchy)&&(!bagUI.activeInHierarchy))
            // {
            //     if(!pauseMenuUI.activeInHierarchy)
            //     {
            //         //pause game
            //         GetComponent<PauseMenu>().Pause();
            //         this.mouseOverMenuUi = true;
            //     }
            //     else
            //     {
            //         //resume game
            //         GetComponent<PauseMenu>().Resume();
            //         this.mouseOverMenuUi = false;
            //     }
            // }
            // if((!pauseMenuUI.activeInHierarchy)&&(bagUI.activeInHierarchy))
            // {
            //     //close bag
            //     inventory.GetComponent<UIInventory>().CloseBag();
            //     this.bagOpen = false;
            //     this.mouseOverBagUI = false;
                
            //     if(!craftingUI.activeInHierarchy)
            //     {
            //         PlayerController.SetAllowedToMove(true);
            //     }
            // }
            // if((!pauseMenuUI.activeInHierarchy)&&(craftingUI.activeInHierarchy))
            // {
            //     //close crafting
            //     canvas.GetComponent<UICrafting>().CloseCrafting();
            //     this.craftingOpen = false;
            //     this.mouseOverCraftUI = false;
            //     if(!bagUI.activeInHierarchy)
            //     {
            //         PlayerController.SetAllowedToMove(true);
            //     }
            // }
        }

        // if((Input.GetKeyDown(KeyCode.E))&&(!pauseMenuUI.activeInHierarchy))
        // {
        //     if(!bagUI.activeInHierarchy)
        //     {
        //         //open bag
        //         inventory.GetComponent<UIInventory>().OpenBag();
        //         this.bagOpen = true;
        //         PlayerController.SetAllowedToMove(false);
        //     }
        //     else
        //     {
        //         //close bag
        //         inventory.GetComponent<UIInventory>().CloseBag();
        //         this.bagOpen = false;
        //         this.mouseOverBagUI = false;
        //         if(!craftingUI.activeInHierarchy)
        //         {
        //             PlayerController.SetAllowedToMove(true);
        //         }
        //     }
        // }

        // if((Input.GetKeyDown(KeyCode.Q))&&(!pauseMenuUI.activeInHierarchy))
        // {
        //     if(!craftingUI.activeInHierarchy)
        //     {
        //         //open crafting
        //         canvas.GetComponent<UICrafting>().OpenCrafting();
        //         this.craftingOpen = true;
        //         PlayerController.SetAllowedToMove(false);
        //     }
        //     else
        //     {
        //         //close crafting
        //         canvas.GetComponent<UICrafting>().CloseCrafting();
        //         this.craftingOpen = false;
        //         this.mouseOverCraftUI = false;
        //         if(!bagUI.activeInHierarchy)
        //         {
        //             PlayerController.SetAllowedToMove(true);
        //         }
        //     }
        // }
    }

    public bool IsUIOpen()
    {
        // if((this.bagOpen)||(this.craftingOpen))
        // {
        //     return true;
        // }
        // return false;

        if(this.mainHUDopen)
        {
            return true;
        }
        return false;
    }

    public bool IsMouseOverUI()
    {
        // if((this.mouseOverHotBarUI)||(this.mouseOverBagUI)||(this.mouseOverCraftUI)||(this.mouseOverMenuUi))
        // {
        //     return true;
        // }
        // return false;
        if((this.mouseOverHotBarUI)||(this.mouseOverMainHUDUI)||(this.mouseOverMenuUi))
        {
            return true;
        }
        return false;
    }

    public void Save()
    {
        GameEvents.OnPreSaveInitiated();
        GameEvents.OnSaveInitiated();
    }

    public void Load()
    {
        GameEvents.OnLoadInitiated();
    }
}
