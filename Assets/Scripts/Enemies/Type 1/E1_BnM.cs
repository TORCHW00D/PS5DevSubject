//Thomas Wilson
//Assignment 2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_BnM : EnemyBaseScript
{
    [SerializeField] GameObject RockAttack;
    void Start()
    {
        Health = 20;
    }
    protected override void Enemy_Attack()
    {
        if (AttackCooldown + AttackTiming < Time.time)
        {
            AttackCooldown = Time.time;
            GameObject Rock = Instantiate(RockAttack, gameObject.transform.position, transform.rotation);
            RockScript rock = Rock.GetComponent<RockScript>();
            rock.SetTrajectory((PlayerChar.transform.position - gameObject.transform.position).normalized);
            base.Enemy_Attack();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("AoE_Damage"))
        {
            TakeDamage(5);
            //Debug.Log("Collision!");
            //RB.velocity = collision.gameObject.transform.position - gameObject.transform.position * MovementSpeedCap;
        }
    }
}
