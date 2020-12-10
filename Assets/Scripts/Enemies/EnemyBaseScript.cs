//Thomas Wilson
//I hate inheritance, but we'll see what we can do with some clever fuckery
//2020-11-21
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    [SerializeField] protected GameObject PlayerChar;
    
    protected int Health = 100;

    protected float MovementSpeedCap = 3.0f;
    protected Vector2 MovementDirection;

    [SerializeField] protected float ViewDistance;
    [SerializeField] protected float AttackDistance;
    [SerializeField] protected float AttackTiming;
    protected float WanderCooldown;
    [SerializeField] Rigidbody2D RB;

    [SerializeField] protected Animator Animator_Enemy;
    
    protected enum StateMachine
    {
        Wander, Hunt, Flee, Attack
    }

    protected StateMachine status;
    protected int AtkStatus;
    protected float AttackCooldown;

    

    // Start is called before the first frame update
    void Awake()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
        if (RB == null)
            Debug.LogError("Couldn't find rigid body on " + gameObject.name);
        
        status = StateMachine.Wander;
        WanderCooldown = Time.time + 5.0f;

        PlayerChar = GameObject.Find("Char");
    }

    // Update is called once per frame
    void Update()
    {
        //process statemachine code for Enemy AI
        if( Vector3.Distance(gameObject.transform.position, PlayerChar.transform.position) <= AttackDistance && 
            Vector3.Distance(gameObject.transform.position, PlayerChar.transform.position) > 1.0f &&
            status != StateMachine.Attack)
        {
            //Debug.Log("Atacking!");
            status = StateMachine.Attack;
        }
        else if(Vector3.Distance(gameObject.transform.position, PlayerChar.transform.position) < ViewDistance &&
            status != StateMachine.Attack)
        {
            //Debug.Log("Hunting!");
            status = StateMachine.Hunt;
        }
        else
        {
            //Debug.Log("Wandering.");
            status = StateMachine.Wander;
        }

      


        //perform calculations on enemy based off state machine
        switch(status)
        {
            case StateMachine.Wander:
                if (WanderCooldown + 5.0 < Time.time)
                {
                    MovementDirection = Random.insideUnitCircle;
                    WanderCooldown = Time.time;
                }
                MovementDirection = MovementDirection.normalized;
                MovementDirection *= MovementSpeedCap;
                break;

            case StateMachine.Hunt:
                MovementDirection = PlayerChar.transform.position - gameObject.transform.position; //aim at the player
                MovementDirection = MovementDirection.normalized;                                  //make it somewhat fair
                MovementDirection *= MovementSpeedCap;                                             //FULL FUCKEN SPEED YEET
                break;

            case StateMachine.Flee:
                MovementDirection = PlayerChar.transform.position - gameObject.transform.position; //aim at the player
                MovementDirection = MovementDirection.normalized;                                  
                MovementDirection = -1 * MovementDirection;                                        //full tilt away from player w/ -1 speed
                MovementDirection *= MovementSpeedCap;                                             //FULL FUCKEN SPEED YEET

                break;

            case StateMachine.Attack:
                Enemy_Attack();
                Animator_Enemy.SetBool("isAttacking", false);
                break;
        }
        RB.velocity = MovementDirection;
        if (RB.velocity.magnitude > MovementSpeedCap)
            RB.velocity.Scale(new Vector2(0.9f, 0.9f));
        if(RB.velocity.magnitude > 0.0f)
        {
            Animator_Enemy.SetFloat("Speed", 1.0f);
            if(RB.velocity.x > 0.05)
            {
                Animator_Enemy.SetFloat("Horizontal", 1.0f);
            }
            else if(RB.velocity.x < -0.05)
            {
                Animator_Enemy.SetFloat("Horizontal", -1.0f);
            }
            if(RB.velocity.y > 0.05)
            {
                Animator_Enemy.SetFloat("Vertical", 1.0f);
            }
            else if (RB.velocity.y < -0.05)
            {
                Animator_Enemy.SetFloat("Vertical", -1.0f);
            }
        }
        else
        {
            Animator_Enemy.SetFloat("Speed", 0.0f);
            Animator_Enemy.SetFloat("Vertical", 0.0f);
            Animator_Enemy.SetFloat("Horizontal", 0.0f);
        }
        //bad kill function, but it is what it is.
        if(Health <= 0)
        {
            GameObject.Find("Level Management System").GetComponent<LevelManager>().UpdateScore(10);
            Destroy(gameObject);
        }

        
    }

    protected virtual void Enemy_Attack() { }

    public void TakeDamage(int DamageTaken)
    {
        ///Negative numbers heal, can heal beyond "max" health
        Health -= DamageTaken;
        status = StateMachine.Flee;
    }
}
