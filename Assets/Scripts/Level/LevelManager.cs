﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int MapSize = 11;
    public GameObject Player;

    private int EntryDoor;

    private GameObject[] levelCollection;


    private struct Room
    {
        public GameObject room;
        public bool Walked;
    }

    private Room[][] CurrentLevelLoaded;

    private Vector2 RoomCoord;

    private enum MovementDirectionForLoad
    {
        up, right, down, left, stop
    }

    private MovementDirectionForLoad movementDirection;

    void Start()
    {
        CurrentLevelLoaded = new Room[MapSize][];
        for(int i = 0; i < MapSize; i++)
        {
            CurrentLevelLoaded[i] = new Room[MapSize];
        }

        levelCollection = Resources.LoadAll<GameObject>("Levels");

        Debug.Log(levelCollection.Length + " levels loaded");

        RoomCoord = new Vector2(MapSize / 2, MapSize / 2);
        CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room = Instantiate(levelCollection[0], transform);
        CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].Walked = true;
        //Debug.Log("Spawned new room @ + " + RoomCoord.x + " " + RoomCoord.y);

        movementDirection = MovementDirectionForLoad.stop;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            movementDirection = MovementDirectionForLoad.up;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movementDirection = MovementDirectionForLoad.right;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movementDirection = MovementDirectionForLoad.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movementDirection = MovementDirectionForLoad.left;
        }
        
        if(movementDirection != MovementDirectionForLoad.stop)
        {
            int JumpDoor = 0;
            CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.SetActive(false);
            //movement calculations for directional
            switch (movementDirection)
            {
                case MovementDirectionForLoad.up:
                    if(RoomCoord.y > 0)
                    {
                        RoomCoord.y -= 1;
                        JumpDoor = 2;
                    }
                    break;
                case MovementDirectionForLoad.left:
                    if(RoomCoord.x > 0)
                    {
                        RoomCoord.x -= 1;
                        JumpDoor = 1;
                    }
                    break;
                case MovementDirectionForLoad.down:
                    if(RoomCoord.y < CurrentLevelLoaded[0].Length - 1)
                    {
                        RoomCoord.y += 1;
                        JumpDoor = 0;
                    }
                    break;
                case MovementDirectionForLoad.right:
                    if(RoomCoord.x < CurrentLevelLoaded[0].Length - 1)
                    {
                        RoomCoord.x += 1;
                        JumpDoor = 3;
                    }
                    break;
            }

            

            //if we haven't loaded a room already, then load a new one
            if (CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].Walked != true) //!= true as we haven't set to false
            {
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].Walked = true; //set the room as walked
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room = Instantiate( levelCollection[Random.Range(0, levelCollection.Length)], transform); //and spawn the new shit
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.SetActive(true); //should be redundant, but we never know.
                //Debug.Log("Spawned new room @ + " + RoomCoord.x + " " + RoomCoord.y);
            }
            else
            {
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.SetActive(true);
            }
            //if this works I'm probably gonna fucken cry omg
            Player.transform.position = GameObject.Find(CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.name + "/" + JumpDoor.ToString()).transform.position;
            Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

            movementDirection = MovementDirectionForLoad.stop;
        }


    }
}