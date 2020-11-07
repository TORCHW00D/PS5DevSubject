//Thomas Wilson
//Assignment 2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 Trajectory;
    public int Damage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTrajectory(Vector2 aim)
    {
        Trajectory = aim;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Trajectory;
        Destroy(gameObject, 4.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.name == "Char")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}
