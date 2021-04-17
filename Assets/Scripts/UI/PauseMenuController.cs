using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static bool gameIsPaused;

    public GameObject pauseMenu;

    private void Awake()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SharedInfo.InitialScenePlayed == true)
        {
            gameIsPaused = !gameIsPaused;   
            PauseGame();
        }
    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        SharedInfo.RestartInfo();
        ZoneManager.LoadZone(0);
    }
}
