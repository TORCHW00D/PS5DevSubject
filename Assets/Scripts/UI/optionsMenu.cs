using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void BackButton()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
