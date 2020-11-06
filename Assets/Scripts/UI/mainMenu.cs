using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem;
public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update


    //// Update is called once per frame
    ////PlayerControls controls;
    //Vector2 movement;
    //public void Awake()
    //{
    //    controls = new PlayerControls();

    //    controls.MenuInteractions.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
    //    controls.MenuInteractions.Move.canceled += ctx => movement = Vector2.zero;
    //}

    void select()
    {

    }
    
    public void LoadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

       public void quitGame()
        {
            Application.Quit();
            Debug.Log("quit!");
        }


    
}
