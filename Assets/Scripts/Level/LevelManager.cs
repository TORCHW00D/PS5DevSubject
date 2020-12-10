//Thomas Wilson
//Assignment 2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;

public class LevelManager : MonoBehaviour
{
    public int currentScore = 0;
    public int highscore = 0;
    public TMP_Text uiScore;
    public TMP_Text uiHiScore;
    private string UISCORESTRING;

    public int MapSize = 11;
    public GameObject Player;
    [SerializeField] GameObject[] EnemyPrefabs;
    private int EntryDoor;

    private GameObject[] levelCollection;
    private static char[] SpawnPos = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

    private struct Room
    {
        public GameObject room;
        public bool Walked;
    }

    private Room[][] CurrentLevelLoaded;

    private Vector2 RoomCoord;

    public enum MovementDirectionForLoad
    {
        up, right, down, left, stop
    }

    private MovementDirectionForLoad movementDirection;

    void Start()
    {
        uiHiScore.text = "Highscore: " + PlayerPrefs.GetInt("Score").ToString();
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
        if(EnemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemies loaded by LevelManager Script! Consider contacting god.");
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MovementSystem(MovementDirectionForLoad movement)
    {
        movementDirection = movement;
        if (movementDirection != MovementDirectionForLoad.stop )
        {
            int JumpDoor = 0;
            CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.SetActive(false);
            //movement calculations for directional
            switch (movementDirection)
            {
                case MovementDirectionForLoad.up:
                    if (RoomCoord.y > 0)
                    {
                        RoomCoord.y -= 1;
                        JumpDoor = 2;
                    }
                    break;
                case MovementDirectionForLoad.left:
                    if (RoomCoord.x > 0)
                    {
                        RoomCoord.x -= 1;
                        JumpDoor = 1;
                    }
                    break;
                case MovementDirectionForLoad.down:
                    if (RoomCoord.y < CurrentLevelLoaded[0].Length - 1)
                    {
                        RoomCoord.y += 1;
                        JumpDoor = 0;
                    }
                    break;
                case MovementDirectionForLoad.right:
                    if (RoomCoord.x < CurrentLevelLoaded[0].Length - 1)
                    {
                        RoomCoord.x += 1;
                        JumpDoor = 3;
                    }
                    break;
            }

            //if we haven't loaded a room already, then load a new one
            if (CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].Walked != true) //!= true as we haven't set to false
            {
                UpdateScore(100);
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].Walked = true; //set the room as walked
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room = Instantiate(levelCollection[Random.Range(0, levelCollection.Length)], transform); //and spawn the new shit
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.SetActive(true); //should be redundant, but we never know.
                //Debug.Log("Spawned new room @ + " + RoomCoord.x + " " + RoomCoord.y);

                //load enemies here, cause this is only called when we're loading a new room
                List<GameObject> tempEnemyBucket = new List<GameObject>();
                for(int i = 0; i < Random.Range(1,9); i++)
                {
                    tempEnemyBucket.Add(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)]); //add a random enemy for new rooms, between 1 & 8 count.
                }
                for(int i = 0; i < tempEnemyBucket.Count; i++)
                {
                    // set the transform for enemy (i) to the Current level's spawn point between a and h
                    tempEnemyBucket[i].transform.position = GameObject.Find(CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.name + "/" + SpawnPos[i].ToString()).transform.position;
                    tempEnemyBucket[i] = Instantiate(tempEnemyBucket[i]);
                    tempEnemyBucket[i].name = "Enemy";
                }
                tempEnemyBucket.Clear();
            }
            else
            {
                CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.SetActive(true);
            }
            //if this works I'm probably gonna fucken cry omg
            Player.transform.position = GameObject.Find(CurrentLevelLoaded[(int)RoomCoord.x][(int)RoomCoord.y].room.name + "/" + JumpDoor.ToString()).transform.position;
            Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            movementDirection = MovementDirectionForLoad.stop;
        }
    }

    public Vector2 GetRoomNumber()
    {
        return RoomCoord;
    }

    public void UpdateScore(int amount)
    {
        currentScore += amount;
        UISCORESTRING = "Score : " + currentScore.ToString();
    }

    private void OnGUI()
    {
        uiScore.text = UISCORESTRING;
    }

    public void OnDieSaveMyScore()
    {
        if(currentScore > PlayerPrefs.GetInt("Score"))
            PlayerPrefs.SetInt("Score", currentScore);
    }

}
