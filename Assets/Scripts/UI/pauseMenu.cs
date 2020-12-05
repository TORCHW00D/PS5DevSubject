//Saugat KC

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseFirstButton, optionsFirstButton, optionsClosedButton;

    public GameObject PauseMenu;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape )) 
        {
            if(isGamePaused )
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
       
    }


    void resumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }


    void pauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }


    public void quitGame()
    {
        Application.Quit(); // perhaps a pause / stop playback in editor function?

        Debug.Log("quit!"); //safety system 
    }
}
