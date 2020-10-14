using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFollower : MonoBehaviour
{
    public GameObject CharToFollow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.position = new Vector3( CharToFollow.transform.position.x, CharToFollow.transform.position.y, -10.0f);
        
    }
}
