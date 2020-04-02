﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!UIInventory.inventoryIsOpen)
            {
                if(GameIsPaused)
                {
                    Resume();
                }else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        //Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        //unpause game
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void SaveGame()
    {
        SaveSystem.SavePlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        //GameEvents.OnSaveInitiated();
    }
}
