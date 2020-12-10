using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementalSystem : MonoBehaviour
{

    [SerializeField] GameObject[] GameElements;
    private GameObject[] ElementStartLocation;
    // Start is called before the first frame update
    void Start()
    {
        ElementStartLocation = new GameObject[GameElements.Length];
        for(int i = 0; i < GameElements.Length; i++)
        {
            ElementStartLocation[i] = new GameObject();
        }
        for(int i = 0; i < GameElements.Length; i++)
        {
            ElementStartLocation[i].transform.position = GameElements[i].transform.position;
        }
        Debug.Log(ElementStartLocation.Length + " element loctions stored!");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Right Bumper") || Input.GetKeyDown(KeyCode.E))
        {
            RotateRight();
        }
        if(Input.GetButtonDown("Left Bumper") || Input.GetKeyDown(KeyCode.Q))
        {
            RotateLeft();
        }
    }

    //something in here is broken, we're losing top position on rotate
    private void RotateRight()
    {
        for(int i = 0; i < GameElements.Length; i++)
        {
            if(i != GameElements.Length - 1)
                GameElements[i].transform.position = ElementStartLocation[i + 1].transform.position;
            else
            {
                GameElements[i].transform.position = ElementStartLocation[0].transform.position;
            }
        }
        GameObject tempBufferObj = GameElements[3];
        GameElements[3] = GameElements[2];
        GameElements[2] = GameElements[1];
        GameElements[1] = GameElements[0];
        GameElements[0] = tempBufferObj;

    }

    //this whole function is broken; we're stacking on position 1
    private void RotateLeft()
    {
        GameElements[0].transform.position = ElementStartLocation[3].transform.position;
        GameElements[1].transform.position = ElementStartLocation[0].transform.position;
        GameElements[2].transform.position = ElementStartLocation[1].transform.position;
        GameElements[3].transform.position = ElementStartLocation[2].transform.position;
        //re-structure order
        GameObject tempBufferObj = GameElements[0];
        GameElements[0] = GameElements[1];
        GameElements[1] = GameElements[2];
        GameElements[2] = GameElements[3];
        GameElements[3] = tempBufferObj;

    }

}
