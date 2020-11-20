using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO:
/* - animation implementation over sprite swapping by code
 * - attack animations / attack variations / sprites
 * - UI polish
 * - Better enemies
 *   - Enemy managment & spawning
 *   - Enemy types
 *   - Enemy attacks
 * - PS4 implementation & controls 
 */
public class LazerAttack : MonoBehaviour
{
    private GameObject lazerHitPoint, lazerSpawnPoint;
    private LineRenderer LazerBeam;
    [SerializeField] protected Material mat;
    void Start()
    {
        lazerHitPoint = new GameObject();
        lazerHitPoint.name = "LazerHitMarker";
        lazerHitPoint.SetActive(false);

        lazerSpawnPoint = new GameObject();
        lazerSpawnPoint.name = "Lazer Spawn";
        lazerSpawnPoint.gameObject.transform.position = gameObject.transform.position;

        

        LazerBeam = gameObject.AddComponent<LineRenderer>();
        LazerBeam.enabled = false;
        LazerBeam.startWidth = 0.1f;
        LazerBeam.endWidth = 0.1f;
        LazerBeam.material = mat;
        LazerBeam.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject FireLazer(Vector2 LazerAimDirection)
    {
        RaycastHit2D HitPoint = Physics2D.Raycast(gameObject.transform.position, LazerAimDirection); //fire lazer and find what it hits
        lazerHitPoint.SetActive(true);                                                          //toggle active for the hitpoint
        lazerHitPoint.transform.position = new Vector3(HitPoint.point.x, HitPoint.point.y, gameObject.transform.position.z); //set hit point to where we hit

        //Debug.DrawRay(gameObject.transform.position, transform.up, Color.green);

        LazerBeam.enabled = true;                                                                    //toggle line renderer
        LazerBeam.SetPosition(0, gameObject.transform.position);                                     //start @ body
        LazerBeam.SetPosition(1, HitPoint.point);                                                    //finish @ collision point
        if (HitPoint.collider.gameObject != null)
            return HitPoint.collider.gameObject;
        else
            return null;
    }

    public void StopLazer()
    {
        LazerBeam.enabled = false;
    }

}
