using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementalSystem : MonoBehaviour
{

    [SerializeField] GameObject[] GameElements;
    private List<Transform> ElementStartLocation;
    // Start is called before the first frame update
    void Start()
    {
        ElementStartLocation = new List<Transform>();
        for(int i = 0; i < GameElements.Length; i++)
        {
            ElementStartLocation.Add(GameElements[i].transform);
        }
        Debug.Log(ElementStartLocation.Count + " element loctions stored!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Right Bumper") || Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Right bumper!");
            RotateRight();
        }
        if(Input.GetButtonDown("Left Bumper") || Input.GetKeyDown(KeyCode.Q))
        {
            RotateLeft();
            Debug.Log("Left bumper!");
        }
    }

    private void RotateRight()
    {
        for(int i = 0; i < GameElements.Length; i++)
        {
            if (i + 1 > GameElements.Length-1)
            {
                GameElements[i].transform.position = ElementStartLocation[0].position;
            }
            else
            {
                GameElements[i].transform.position = ElementStartLocation[i + 1].position;
            }
        }
    }

    private void RotateLeft()
    {
        for(int i = 0; i < GameElements.Length; i++)
        {
            if(i - 1 < 0)
            {
                GameElements[i].transform.position = ElementStartLocation[GameElements.Length - 1].position;
            }
            else
            {
                GameElements[i].transform.position = ElementStartLocation[i - 1].position;
            }
        }
    }

}
