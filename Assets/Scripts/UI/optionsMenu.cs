//Saugat KC

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class optionsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    //public GameObject OptionsMenu;
    public GameObject   optionsClosedButton;
    void Start()
    {
        
        gameObject.SetActive(false);
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

  

    // Update is called once per frame
    public void BackButton()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
       EventSystem.current.SetSelectedGameObject(null);
       EventSystem.current.SetSelectedGameObject(optionsClosedButton);
    }
}
