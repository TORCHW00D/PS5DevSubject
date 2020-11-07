//Thomas Wilson
//Assignment 2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_BnM : MonoBehaviour
{
    public GameObject PlayerChar;
    public GameObject Rock;
    private RockScript RockAim;
    private Vector2 MovementDirection;
    public float MovementSpeedCap = 5.0f;
    private float WanderCooldown;
    private Rigidbody2D RB;

    private SpriteRenderer Enemy;
    public Sprite[] EnemyBody;
    public Sprite[] EnemyAttack;
    private float SpriteUpdate;
    private int CurrentSprite;
    private bool isFacingRight;
    public enum EnemyType1State
    {
        Wander, Hunt, Flee, Attack
    }

    private EnemyType1State status;
    private int AtkStatus;
    private float AttackCooldown;
    void Start()
    {
        Enemy = GetComponent<SpriteRenderer>();
        if(EnemyBody.Length == 0 || EnemyAttack.Length == 0)
        {
            Debug.LogError("Catastrophic failure; couldn't locate enemy Type 1 sprites!");
        }
        CurrentSprite = 0;
        Enemy.sprite = EnemyBody[CurrentSprite];
        SpriteUpdate = Time.time;
        isFacingRight = true;

        RB = GetComponent<Rigidbody2D>();
        status = EnemyType1State.Wander;
        WanderCooldown = Time.time;
        AttackCooldown = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, PlayerChar.transform.position) <= 3.0f && Vector3.Distance(gameObject.transform.position, PlayerChar.transform.position) > 1.0f && status != EnemyType1State.Attack)
        {
            status = EnemyType1State.Attack;
        }
        else if (Vector3.Distance(gameObject.transform.position, PlayerChar.transform.position) < 5.0f && status != EnemyType1State.Attack)
        {
            status = EnemyType1State.Hunt;
        }
        else
        {
            status = EnemyType1State.Wander;
        }

        switch(status)
        {
            case EnemyType1State.Wander:
                if (WanderCooldown + 5.0 < Time.time)
                {
                    MovementDirection = Random.insideUnitCircle;
                    WanderCooldown = Time.time;
                }
                MovementDirection = MovementDirection.normalized;
                MovementDirection *= MovementSpeedCap;
                break;

            case EnemyType1State.Hunt:
                MovementDirection = PlayerChar.transform.position - gameObject.transform.position; //aim at the player
                MovementDirection = MovementDirection.normalized;                                  //make it somewhat fair
                MovementDirection *= MovementSpeedCap;                                             //FULL FUCKEN SPEED YEET
                break;

            case EnemyType1State.Flee:


                break;

            case EnemyType1State.Attack:
                status = EnemyType1State.Hunt;
                if (AttackCooldown + 3.0f < Time.time)
                {
                    GameObject temp = Instantiate(Rock, gameObject.transform.position, gameObject.transform.rotation);
                    RockAim = temp.GetComponent<RockScript>();
                    Vector2 AimDir = PlayerChar.transform.position - gameObject.transform.position;
                    RockAim.SetTrajectory(AimDir);
                    AttackCooldown = Time.time;
                }
                break;
        }
       
        RB.velocity = MovementDirection;

        if(SpriteUpdate + 0.2f < Time.time && status != EnemyType1State.Attack)
        {
            CurrentSprite++;
            SpriteUpdate = Time.time;
            if (CurrentSprite >= EnemyBody.Length-1)
                CurrentSprite = 0;

            if (MovementDirection.x < 0 && isFacingRight)
            {
                isFacingRight = !isFacingRight;
                Vector3 tempscale = transform.localScale;
                tempscale.x *= -1;
                transform.localScale = tempscale;
            }
            if(MovementDirection.x > 0 && !isFacingRight)
            {
                isFacingRight = !isFacingRight;
                Vector3 tempscale = transform.localScale;
                tempscale.x *= -1;
                transform.localScale = tempscale;
            }

            Enemy.sprite = EnemyBody[CurrentSprite];
        }
        else if(SpriteUpdate + 0.2f < Time.time && status == EnemyType1State.Attack)
        {
            CurrentSprite = 0;
            SpriteUpdate = Time.time;
            if (MovementDirection.x < 0 && isFacingRight)
            {
                isFacingRight = !isFacingRight;
                Vector3 tempscale = transform.localScale;
                tempscale.x *= -1;
                transform.localScale = tempscale;
            }
            if (MovementDirection.x > 0 && !isFacingRight)
            {
                isFacingRight = !isFacingRight;
                Vector3 tempscale = transform.localScale;
                tempscale.x *= -1;
                transform.localScale = tempscale;
            }
            Enemy.sprite = EnemyAttack[CurrentSprite];
        }
    }
}
