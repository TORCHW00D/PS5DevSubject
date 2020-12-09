//Saugat KC
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public int playerHighScore = 0;
    public int playerCurrentScore = 0;

    public string playerName = "Player";

    public float gameTimer = 5f;
    public bool gameExited = false;

    // Start is called before the first frame update
    void Start()
    {
        playerHighScore = PlayerPrefs.GetInt("HighScore");

        playerName = PlayerPrefs.GetString("PlayerName");

        print(" Current Highscore : " + playerHighScore);
        print("Player's name : " + playerName);
    }

    // Update is called once per frame
    void Update()
    {
        //need a if loop to find out if the emeny is dead to add score

        gameTimer = Time.deltaTime;
        if(gameTimer <= 0 && gameExited != true)
        {

            gameExited = true;


            if(playerCurrentScore > playerHighScore)
            {
                playerHighScore = playerCurrentScore;
                PlayerPrefs.SetInt("highScore", playerHighScore);
            }
        }
    }
}
