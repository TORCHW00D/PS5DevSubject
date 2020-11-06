using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem;
public class mainMenu : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject Crying;
    public void Start()
    {
        Time.timeScale = 0.0f;
    }

    
    public void LoadGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //requesting clarification on this
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void LoadOptionsMenu()
    {
        gameObject.SetActive(false);
        OptionsMenu.SetActive(true);
    }


    public void LoadCryingButton()
    {
        Crying.SetActive(true);
        gameObject.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit(); // perhaps a pause / stop playback in editor function?

        Debug.Log("quit!"); //safety system 
    }
}
