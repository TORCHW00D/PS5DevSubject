using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementDir
    {
        up,
        right,
        down,
        left
    }

    public GameObject pauseMenu;

    public GameObject NextDoor, LockedDoor;

    private bool IsInDoorway;
    private int doorwayNumer;
    private float DoorLock;
    private bool IsDoorLocked;

    private MovementDir DirectionMoved;

    public SpriteRenderer Torso;
    public SpriteRenderer Legs;

    public float MaxSpeed = 5.0f;

    public Sprite[] TORSOpiecesLeft;
    public Sprite[] TORSOpiecesRight;
    public Sprite[] TORSOpiecesUp;
    public Sprite[] TORSOpiecesDown;
    public Sprite[] LEGpiecesLeft;
    public Sprite[] LEGpiecesRight;
    public Sprite[] LEGpiecesUp;
    public Sprite[] LEGpiecesDown;

    private Rigidbody2D CharBody;
    public LevelManager globalLevelManagement;

    private float SpriteUpdate;
    private int CurrentSprite = 0;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        NextDoor.SetActive(false);
        LockedDoor.SetActive(false);
        CharBody = GetComponent<Rigidbody2D>();
        if(TORSOpiecesRight.Length == 0  || TORSOpiecesLeft.Length == 0  || TORSOpiecesUp.Length == 0  || TORSOpiecesDown.Length == 0)
        {
            Debug.LogError("Catastrophic failure; missing some TORSO sprites for character.");
        }

        if(LEGpiecesRight.Length == 0 || LEGpiecesLeft.Length == 0 || LEGpiecesUp.Length == 0 || LEGpiecesDown.Length == 0)
        {
            Debug.LogError("Catastrophic failure; missing some LEG sprites for character.");
        }

        Torso.sprite = TORSOpiecesDown[0];
        Legs.sprite = LEGpiecesDown[0];
        SpriteUpdate = Time.time;
        DoorLock = Time.time;

        IsInDoorway = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") > 0.05 || Input.GetAxis("Vertical") < -0.05)
        {
            //gameObject.transform.position += new Vector3(0.0f, 0.1f, 0.0f) * Input.GetAxis("Vertical");
            CharBody.velocity += new Vector2(0.0f, 10.0f) * Input.GetAxis("Vertical") * Time.deltaTime;
            if(Input.GetAxis("Vertical") > 0.05)
            {
                DirectionMoved = MovementDir.up;
            }
            if(Input.GetAxis("Vertical") < -0.05)
            {
                DirectionMoved = MovementDir.down;
            }

        }
        if(Input.GetAxis("Horizontal") > 0.05 || Input.GetAxis("Horizontal") < -0.05)
        {
            if(Input.GetAxis("Horizontal") > 0.05)
            {
                DirectionMoved = MovementDir.right;
            }
            if(Input.GetAxis("Horizontal") < -0.05)
            {
                DirectionMoved = MovementDir.left;
            }

            //gameObject.transform.position += new Vector3(0.1f, 0.0f, 0.0f) * Input.GetAxis("Horizontal");
            CharBody.velocity += new Vector2(10.0f, 0.0f) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.E) && IsInDoorway)
        {
            globalLevelManagement.MovementSystem((LevelManager.MovementDirectionForLoad)doorwayNumer);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }

        if(CharBody.velocity.magnitude > MaxSpeed)
        {
            CharBody.velocity.Scale(new Vector2(0.9f, 0.9f));
        }

        switch(DirectionMoved)
        {
            case MovementDir.down:
                Torso.sprite = TORSOpiecesDown[CurrentSprite];
                Legs.sprite = LEGpiecesDown[CurrentSprite];
                break;

            case MovementDir.up:
                Torso.sprite = TORSOpiecesUp[CurrentSprite];
                Legs.sprite = LEGpiecesUp[CurrentSprite];
                break;

            case MovementDir.left:
                Torso.sprite = TORSOpiecesLeft[CurrentSprite];
                Legs.sprite = LEGpiecesLeft[CurrentSprite];
                break;

            case MovementDir.right:
                Torso.sprite = TORSOpiecesRight[CurrentSprite];
                Legs.sprite = LEGpiecesRight[CurrentSprite];
                break;
        }
        if(SpriteUpdate + 0.2f < Time.time)
        {
            CurrentSprite++;
            SpriteUpdate = Time.time;
            if (CurrentSprite >= 4)
                CurrentSprite = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Doorway" && DoorLock + 0.25f < Time.time)
        {
            DoorLock = Time.time;
            IsInDoorway = true;
            string temp = collision.gameObject.name;
            doorwayNumer = Int16.Parse(temp);

            Vector2 currentRoom = globalLevelManagement.GetRoomNumber();

            string DoorCollision = GameObject.Find(collision.gameObject.name).name;

            if (currentRoom.y == 0  &&  DoorCollision == "0") /*|| currentRoom.y == 0 || currentRoom.y == globalLevelManagement.MapSize - 1 || currentRoom.x == globalLevelManagement.MapSize - 1*/
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
            else if(currentRoom.x == 0 && DoorCollision == "3")
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
        if(collision.gameObject.tag == "Doorway")
        {
            IsInDoorway = false;
        }
    }

    private void OnGUI()
    {
        if(IsInDoorway)
        {
            if(!IsDoorLocked)
            {
                NextDoor.SetActive(true);
            }
            else
            {
                LockedDoor.SetActive(true);
            }
        }
        else
        {
            NextDoor.SetActive(false);
            LockedDoor.SetActive(false);
        }
    }

}
