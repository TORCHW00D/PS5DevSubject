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

    private float SpriteUpdate;
    private int CurrentSprite = 0;

    // Start is called before the first frame update
    void Start()
    {
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
        if(SpriteUpdate + 0.5f < Time.time)
        {
            CurrentSprite++;
            SpriteUpdate = Time.time;
            if (CurrentSprite >= 4)
                CurrentSprite = 0;
        }
    }
}
