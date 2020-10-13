using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private bool FacingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") > 0.05 || Input.GetAxis("Vertical") < -0.05)
        {
            gameObject.transform.position += new Vector3(0.0f, 0.1f, 0.0f) * Input.GetAxis("Vertical");
        }
        if(Input.GetAxis("Horizontal") > 0.05 || Input.GetAxis("Horizontal") < -0.05)
        {
            if (!FacingRight && Input.GetAxis("Horizontal") > 0.05)
            {
                FacingRight = !FacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
            if (FacingRight && Input.GetAxis("Horizontal") < -0.05)
            {
                FacingRight = !FacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
            gameObject.transform.position += new Vector3(0.1f, 0.0f, 0.0f) * Input.GetAxis("Horizontal");
        }
    }
}
