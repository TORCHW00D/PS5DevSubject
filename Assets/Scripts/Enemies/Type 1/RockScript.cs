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
    private float HoldTimeUntilDamage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HoldTimeUntilDamage = Time.time;
    }

    public void SetTrajectory(Vector2 aim)
    {
        Trajectory = aim * 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Trajectory;
        Destroy(gameObject, 4.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.name == "Char" && HoldTimeUntilDamage + 0.1f < Time.time)
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.TakeDamage(Damage);
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }
}
