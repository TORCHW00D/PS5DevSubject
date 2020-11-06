using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField] GameObject PauseMenu;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape )) //Needs a joystick button
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
    }
}
