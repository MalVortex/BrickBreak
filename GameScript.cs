using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameScript : MonoBehaviour
{

    public int lives = 5;
    //public int bricks = 20;

    int bricks;
    public float resetDelay = 1f;
    public Text livesText;
    public Text scoreText;
    private int currentScore = 0;
    private float scoreVelocityMultiplier = 0.5f;
    //public GameObject gameOver;
    public GameObject youWon;
    public GameObject bricksPrefab;
    public GameObject paddle;
    public GameObject deathParticles;
    public static GameScript instance = null;
    public AudioClip playerWonAudio;

    public bool ballIsLive = false;

    public GameObject phantomBall;
    private GameObject clonedPhantomBall;


    private GameObject clonePaddle;



    //ball booster!
    public Slider boosterSlider;
    public float ballBoosterMaxCharge = 100f;
    public float boosterCurretCharge = 0f;
    public float boosterMultiplier = 5f;
    public float boosterCooldown = 0f;
    public float boosterBaseCooldown = 3f; //should probs always be defined as the same as boosterCooldown
    public float boosterChargeSpeed = 20f;
    public GameObject theBall;
    public Rigidbody theBallBody;



    //Arcade Mode High Score Variables. Just leave these be...

    public float adventureTimeElapsed = 0.0f;
    public float saverTester = 19f;
    public int currentHSOne;
    public int currentHSTwo;
    public int currentHSThree;
    public int currentHSFour;
    public int currentHSFive;

    //Pause Menu Implementation:
    private bool gamePaused = false;
    public GameObject pauseMenu;

    //game over menu implementation

    public GameObject gameOverMenu;







    // Use this for initialization
    void Start()
    {
        //this prevents more than one game managing script from running at the same time
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Setup();


    }

    public void Setup()
    {

        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + currentScore;

        clonePaddle = Instantiate(paddle, paddle.transform.position, Quaternion.identity) as GameObject;
        Instantiate(bricksPrefab, bricksPrefab.transform.position, Quaternion.identity);
        //        Debug.Log("instantiating the environment");
        bricks = bricksPrefab.transform.childCount;

        //booster stuff



        boosterCurretCharge = 0f;
        boosterCooldown = 0f;
        boosterBaseCooldown = 3f;

        if (theBall == null)
        {
            theBall = GameObject.FindGameObjectWithTag("Ball");
            theBallBody = theBall.GetComponent<Rigidbody>();
        }

        //Arcade mode UI
        if (ModeScript.GameMode == "Arcade")
        {
            //turn on the score UI
            scoreText.gameObject.SetActive(true);
            //change our number of lives
            lives = 2;
            livesText.text = "Lives: " + lives;
        }

    }

    void checkGameOver()
    {
        if (bricks < 1)
        {
            youWon.SetActive(true);
            Time.timeScale = 0.25f;
            //Invoke("Reset", resetDelay);
            Invoke("NextLevel", resetDelay);

            AudioSource.PlayClipAtPoint(playerWonAudio, transform.position);
        }
        if (lives < 1)
        {
            Debug.Log("calling game over function");
            GameOver();

        }
    }

    void GameOver()
    {
        Debug.Log("game over now executing");
        gamePaused = true;
        gameOverMenu.SetActive(true);
        Debug.Log("game over menu enabled");
        Time.timeScale = 0.00001f;
        Debug.Log("timescale down to zero sir");
        
    }

    void NextLevel() // this function is going to need to be improved at some point
    {
        Time.timeScale = 1f;

        string currentLevel = Application.loadedLevelName;

        //OK here we go to move to next level

        if (ModeScript.GameMode == "Arcade")
        {
            HighScoreCheck();
            Application.LoadLevel("Main Menu");
        }
        else
        {
            switch (currentLevel)
            {
                case "Level 1":
                    Debug.Log("loading level 2 campaign mode");
                    Application.LoadLevel("Level 2");
                    break;
                case "Level 2":
                    Debug.Log("loading level 3 campaign mode");
                    Application.LoadLevel("Level 3");
                    break;
                case "Level 3":
                    Debug.Log("loading level 4 campaign mode");
                    Application.LoadLevel("Level 4");
                    break;
                case "Level 4":
                    Debug.Log("loading level 5 campaign mode");
                    Application.LoadLevel("Level 5");
                    break;
                case "Level 5":
                    Debug.Log("loading main menu");
                    Application.LoadLevel("Main Menu");
                    break;
                default:
                    Debug.Log("error selecting new level!!");
                    break;
            }


        }



    }

    void Reset()
    {
        Time.timeScale = 1f;
        Application.LoadLevel("Main Menu");
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;
        Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
        Destroy(clonePaddle);

        if (theBall != null)
        {
            Destroy(theBall);
            theBall = null;

        }

        //lets make sure there aren't any secondary balls either

        GameObject[] secondaryBalls = GameObject.FindGameObjectsWithTag("SecondaryBall");
        if (secondaryBalls.Length > 0)
        {
            for (int i = 0; i < secondaryBalls.Length; i++)
            {
                Instantiate(deathParticles, secondaryBalls[i].transform.position, Quaternion.identity);
                Destroy(secondaryBalls[i]);
            }
        }



        checkGameOver();
        Invoke("SetupPaddle", resetDelay);

        ballIsLive = false;

    }

    void SetupPaddle()
    {
        clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
        if (theBall == null)
        {
            theBall = GameObject.FindGameObjectWithTag("Ball");
            theBallBody = theBall.GetComponent<Rigidbody>();
        }
    }

    public void DestroyBrick(int brickValue)
    {
        if (ModeScript.GameMode == "Arcade")
        {
            float finalValue;
            float totalVelocity;
            float xVelocity;
            float yVelocity;

            //lets get the ball's current velocity in absolute terms

            xVelocity = Mathf.Abs(theBallBody.velocity.x);
            yVelocity = Mathf.Abs(theBallBody.velocity.y);

            Debug.Log("ball velocity is: " + xVelocity + ", " + yVelocity);

            //then lets add those velocities together, and dampen the multiplier by the score multiplier
            //lets clamp it to a mininum of 0.1x and 10x so things don't get too crazy in either direction
            totalVelocity = Mathf.Clamp((((xVelocity + yVelocity) * scoreVelocityMultiplier)), 0.1f, 10f);

            Debug.Log("total velocity is: " + totalVelocity);

            finalValue = (float)brickValue * totalVelocity;

            Debug.Log("score to be added is: " + finalValue);


            currentScore += (int)finalValue;
            scoreText.text = "Score: " + currentScore;
        }

        bricks--;
        checkGameOver();

    }

    public void HighScoreCheck()
    {
        // Debug.Log("scoring system not implemented yet");
        SaverScript gameSaver = new SaverScript();

        gameSaver.LoadGame();

        currentHSOne = gameSaver.highScoreOne;
        currentHSTwo = gameSaver.highScoreTwo;
        currentHSThree = gameSaver.highScoreThree;
        currentHSFour = gameSaver.highScoreFour;
        currentHSFive = gameSaver.highScoreFive;


        //lets find out what score to compare now

        string levelToTest = Application.loadedLevelName;

        switch (levelToTest)
        {
            case "Level 1":
                if (currentHSOne < currentScore)
                {
                    currentHSOne = currentScore;
                    Debug.Log("new high score from arcade level 1. high score is: " + currentScore);
                    gameSaver.SaveGame(currentHSOne, currentHSTwo, currentHSThree, currentHSFour, currentHSFive);
                    break;
                }
                else
                {
                    Debug.Log(currentScore + " is less than the current high score of: " + currentHSOne);
                    break;
                }
   
            case "Level 2":
                if (currentHSTwo < currentScore)
                {
                    currentHSTwo = currentScore;
                    Debug.Log("new high score from arcade level 2. high score is: " + currentScore);
                    gameSaver.SaveGame(currentHSOne, currentHSTwo, currentHSThree, currentHSFour, currentHSFive);
                    break;
                }
                else
                {
                    Debug.Log(currentScore + " is less than the current high score of: " + currentHSTwo);
                    break;

                }
                
            case "Level 3":
                if (currentHSThree < currentScore)
                {
                    currentHSThree = currentScore;
                    Debug.Log("new high score from arcade level 3. high score is: " + currentScore);
                    gameSaver.SaveGame(currentHSOne, currentHSTwo, currentHSThree, currentHSFour, currentHSFive);
                    break;
                }
                else
                {
                    Debug.Log(currentScore + " is less than the current high score of: " + currentHSThree);
                    break;
                }
                
            case "Level 4":
                if (currentHSFour < currentScore)
                {
                    currentHSFour = currentScore;
                    Debug.Log("new high score from arcade level 4. high score is: " + currentScore);
                    gameSaver.SaveGame(currentHSOne, currentHSTwo, currentHSThree, currentHSFour, currentHSFive);
                    break;
                }
                else
                {
                    Debug.Log(currentScore + " is less than the current high score of: " + currentHSFour);
                    break;
                }
                
            case "Level 5":
                if (currentHSFive < currentScore)
                {
                    currentHSFive = currentScore;
                    Debug.Log("new high score from arcade level 5. high score is: " + currentScore);
                    gameSaver.SaveGame(currentHSOne, currentHSTwo, currentHSThree, currentHSFour, currentHSFive);
                    break;
                }
                else
                {
                    Debug.Log(currentScore + " is less than the current high score of: " + currentHSFive);
                    break;
                }
                
            default:
                {
                    Debug.Log("error: unable to select correct level to compare scores too! ahh");
                    break;
                }
        }





    }

    public void MoreBalls()
    {
        float xBoost;
        float yBoost;

        xBoost = UnityEngine.Random.RandomRange(-400f, 400f);
        yBoost = UnityEngine.Random.RandomRange(400f, 800f);

        Vector3 forceImparted = new Vector3(xBoost, yBoost, 0f);

        Vector3 spawnPosition = new Vector3(clonePaddle.transform.position.x + 1.5f, clonePaddle.transform.position.y + 3f, 0f);


        clonedPhantomBall = (GameObject)Instantiate(phantomBall, spawnPosition, Quaternion.identity);
        clonedPhantomBall.GetComponent<Rigidbody>().AddForce(forceImparted);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        boosterCooldown += Time.deltaTime;
        boosterCurretCharge += boosterChargeSpeed * Time.deltaTime;

        boosterSlider.value = Mathf.Clamp((100 * boosterCurretCharge / ballBoosterMaxCharge), 0.0f, 100.0f);



        //the ball is in play, the cooldown is ready. HIT THE NOS
        if (Input.GetButtonDown("Jump") == true && ballIsLive == true && boosterCooldown >= boosterBaseCooldown)
        {
            theBallBody.AddForce(0.0f, boosterCurretCharge * boosterMultiplier, 0.0f);
            boosterCooldown = 0.0f;
            boosterCurretCharge = 0.0f;
        }

        //lets implement a simple pause menu to make this truly shine
        //these functions should really all be their own thing called by fixed update
        //and not actually in the fixed update method itself
        //its a bit late to clean up some of the core gameplay code, but we can at least make the pause menu
        //cleanly included

        CheckForPause();



    }

    public void CheckForPause()
    {
        
        if (Input.GetButtonDown("Cancel") == true && gamePaused == false)
        {
            Debug.Log("Pausing the game due to esc pressed");
            Time.timeScale = 0.00001f;
            if (pauseMenu.gameObject != null)
            {
                pauseMenu.SetActive(true);
            }
            else
                Debug.Log("Null object exception: pause menu game object");
        }

        else if (Input.GetButtonDown("Cancel") == true && gamePaused == true) // press escape to exit out of the pause menu and resume gameplay
        {
            Debug.Log("attempting to  unpause the game while paused due to esc pressed");
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
    }

    public void MainMenuClicked()
    {
        Debug.Log("main menu clicked called");
        gamePaused = false;
        Time.timeScale = 1.0f;
        Application.LoadLevel("Main Menu");
    }

    public void ResumeClicked()
    {
        Debug.Log("resume button clicked called");
        gamePaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void TryAgainClicked()
    {
        Debug.Log("try gain clicked called");
        if(ModeScript.GameMode == "Arcade")
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel("Level 1");
        }
    }
}


