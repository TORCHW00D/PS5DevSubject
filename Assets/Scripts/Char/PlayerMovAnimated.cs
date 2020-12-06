//Thomas Wilson
//Assignment 2

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovAnimated : MonoBehaviour
{
    private enum MovementDir
    {
        up,
        right,
        down,
        left
    }

    public LazerAttack Char_Laz_Wep;

    public Slider Char_Health_Bar;
    public Slider AudioSourceVolumeSlider;

    public GameObject Char_Pause_Menu;

    public GameObject Char_Next_Door, Char_Locked_Door;

    private bool IsInDoorway;
    private int doorwayNumer;
    private float DoorLock;
    private bool IsDoorLocked;

    private float AttackCooldown;

    private MovementDir DirectionMoved;

   
    public float MaxSpeed = 5.0f;

    
    private Rigidbody2D CharBody;
    public LevelManager globalLevelManagement;

    private CircleCollider2D HurtCircle;
    private Animator Char_Animator;

    public AudioSource[] SFX;

    private int Health;
    // Start is called before the first frame update
    void Start()
    {
        Char_Pause_Menu.SetActive(false);
        Char_Next_Door.SetActive(false);
        Char_Locked_Door.SetActive(false);

        CharBody = GetComponent<Rigidbody2D>();

        DoorLock = Time.time;
        IsInDoorway = false;

        Health = 100;
        Char_Health_Bar.maxValue = Health;

        AttackCooldown = Time.time;

        GameObject PlayerAoE = new GameObject("PlayerAoE");
        PlayerAoE.transform.SetParent(gameObject.transform);
        PlayerAoE.AddComponent<CircleCollider2D>();
        HurtCircle = PlayerAoE.GetComponent<CircleCollider2D>();
        HurtCircle.radius = 1.0f;
        HurtCircle.offset = new Vector2(0.0f, -0.1f);
        HurtCircle.isTrigger = true;
        PlayerAoE.tag = "AoE_Damage";
        HurtCircle.enabled = false;

        Char_Animator = GetComponent<Animator>();
        //SFX = GetComponent<AudioSource>();
        for(int i = 0; i < SFX.Length; i++)
        {
            SFX[i].mute = true;
        }
        AudioSourceVolumeSlider.minValue = 0.0f;
        AudioSourceVolumeSlider.maxValue = 1.0f;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") > 0.05 || Input.GetAxis("Vertical") < -0.05)
        {
            Char_Animator.SetFloat("Speed", 1.0f);

            if (Input.GetAxis("Vertical") > 0.05)
            {
                DirectionMoved = MovementDir.up;
                Char_Animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
            }
            if (Input.GetAxis("Vertical") < -0.05)
            {
                DirectionMoved = MovementDir.down;
                Char_Animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
            }
            CharBody.velocity += new Vector2(0.0f, 10.0f) * Input.GetAxis("Vertical") * Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") > 0.05 || Input.GetAxis("Horizontal") < -0.05)
        {
            Char_Animator.SetFloat("Speed", 1.0f);

            if (Input.GetAxis("Horizontal") > 0.05)
            {
                DirectionMoved = MovementDir.right;
                Char_Animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            }
            if (Input.GetAxis("Horizontal") < -0.05)
            {
                DirectionMoved = MovementDir.left;
                Char_Animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            }
            CharBody.velocity += new Vector2(10.0f, 0.0f) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }

        if ((Input.GetMouseButton(1) && AttackCooldown + 0.5f < Time.time) || (Input.GetAxis("Secondary Fire") > 0.01f && AttackCooldown + 0.5f < Time.time))
        {
            HurtCircle.enabled = !HurtCircle.enabled;
            IEnumerator coroutine = StopHurtCircle(1.1f);
            StartCoroutine(coroutine);
            AttackCooldown = Time.time;
            Char_Animator.SetBool("isAttacking", true);
        }

        if ((Input.GetMouseButton(0) && AttackCooldown + 0.5f < Time.time) || (Input.GetAxis("Primary Fire") > 0.01f && AttackCooldown + 0.5f < Time.time))
        {
            GameObject Object;
            SFX[1].Play();
            SFX[1].mute = false;
            if(Input.GetAxis("RTS-X") != 0.0f || Input.GetAxis("RTS-Y") != 0.0f)
            {
                Vector2 RTS_aim = new Vector2(Input.GetAxis("RTS-X"), Input.GetAxis("RTS-Y"));
                Object = Char_Laz_Wep.FireLazer(RTS_aim);
            }
            else
            {
                if (CharBody.velocity.magnitude != 0.0)
                    Object = Char_Laz_Wep.FireLazer(CharBody.velocity.normalized);
                else
                    Object = Char_Laz_Wep.FireLazer(gameObject.transform.right);
            }

            if (Object.GetComponent<EnemyBaseScript>())
            {
                Object.GetComponent<EnemyBaseScript>().TakeDamage(15);
            }
            IEnumerator enumerator = StopLazer(0.1f);
            StartCoroutine(enumerator);
            AttackCooldown = Time.time;
        }

        if ((Input.GetKey(KeyCode.E) || Input.GetButton("X")) && IsInDoorway && !IsDoorLocked)
        {
            GameObject temp = GameObject.Find("Enemy");
            if (temp == null)
                globalLevelManagement.MovementSystem((LevelManager.MovementDirectionForLoad)doorwayNumer);
        }

        if (Input.GetKey(KeyCode.Escape) || Input.GetButton("Start"))
        {
            Char_Pause_Menu.SetActive(true);
            Time.timeScale = 0.0f;
        }

        if (CharBody.velocity.magnitude > MaxSpeed)
        {
            CharBody.velocity.Scale(new Vector2(0.9f, 0.9f));
        }

        switch (DirectionMoved)
        {
            case MovementDir.down:
               
                break;

            case MovementDir.up:
                break;

            case MovementDir.left:
                break;

            case MovementDir.right:
                break;
        }
        
        if(CharBody.velocity.magnitude > 0.1f)
        {
            SFX[0].mute = false;
        }
        else
        {
            SFX[0].mute = true;
        }


        if (Health < 0)
        {
            Time.timeScale = 0;
            Char_Pause_Menu.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Doorway" && DoorLock + 1.0f < Time.time)
        {
            DoorLock = Time.time;
            IsInDoorway = true;
            string temp = collision.gameObject.name;
            doorwayNumer = Int16.Parse(temp);

            Vector2 currentRoom = globalLevelManagement.GetRoomNumber();

            string DoorCollision = GameObject.Find(collision.gameObject.name).name;

            if (currentRoom.y == 0 && DoorCollision == "0") /*|| currentRoom.y == 0 || currentRoom.y == globalLevelManagement.MapSize - 1 || currentRoom.x == globalLevelManagement.MapSize - 1*/
            {
                IsDoorLocked = true;
            }
            else if (currentRoom.x == globalLevelManagement.MapSize - 1 && DoorCollision == "1")
            {
                IsDoorLocked = true;
            }
            else if (currentRoom.y == globalLevelManagement.MapSize - 1 && DoorCollision == "2")
            {
                IsDoorLocked = true;
            }
            else if (currentRoom.x == 0 && DoorCollision == "3")
            {
                IsDoorLocked = true;
            }
            else
            {
                IsDoorLocked = false;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Doorway")
        {
            IsInDoorway = false;
        }
    }

    private void OnGUI()
    {
        if (IsInDoorway)
        {
            GameObject temp = GameObject.Find("Enemy");
            if (temp == null)
            {
                if (!IsDoorLocked)
                {
                    Char_Next_Door.SetActive(true);
                }
                else
                {
                    Char_Locked_Door.SetActive(true);
                }
            }
        }
        else
        {
            Char_Next_Door.SetActive(false);
            Char_Locked_Door.SetActive(false);
        }
        Char_Health_Bar.value = Health;
        for(int i = 0; i < SFX.Length; i++)
        {
            SFX[i].volume = AudioSourceVolumeSlider.value;
        }
    }

    public void TakeDamage(int dmg)
    {
        Health -= dmg;
    }

    private IEnumerator StopHurtCircle(float toggletime)
    {
        yield return new WaitForSeconds(toggletime);
        HurtCircle.enabled = false;
        Char_Animator.SetBool("isAttacking", false);
    }

    private IEnumerator StopLazer(float toggletime)
    {
        yield return new WaitForSeconds(toggletime);
        Char_Laz_Wep.StopLazer();
    }

}
