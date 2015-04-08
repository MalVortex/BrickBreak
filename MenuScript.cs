using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour {

    //decs

    public static MenuScript instance = null;

    public Animator tipsAnimator;
    public Animator tipsBlurbAnimator;
    public Animator creditsAnimator;
    public Animator blurbAnimator;
    public Animator obscuringAnimator;

    public AudioClip menuMusic;
    public AudioClip menuButtonPressed;
    public AudioClip creditsMusic;

  //  public AudioSource[] soundSource;
    public AudioSource menuMusicGenerator;
    public AudioSource menuButtonSoundGenerator;
    public AudioSource creditsMusicGenerator;


    //public Text scoreValueOne;
    //public Text scoreValueTwo;
    //public Text scoreValueThree;
    //public Text scoreValueFour;
    //public Text scoreValueFive;

    public Text[] intScoreValue;

    public GameObject obscuringPanel;
    public GameObject tipsBlurb;
    public GameObject creditsBlurb;
    public GameObject[] UILevel1TipsMode;
    public GameObject[] UILevel1CreditsMode;
    public GameObject[] UILevel1;
    public GameObject[] UILevel2;
    public GameObject[] UILevel3;

    private bool UILevel1Enabled = true;
    private bool CreditsModeEnabled = false;
    private bool tipsModeEnabled = false;
    private bool UILevel2Enabled = false;
    private bool UILevel3Enabled = false;
    private bool intScoreValueEnabled = false;

    public int menuHSOne = 0;
    public int menuHSTwo = 0;
    public int menuHSThree = 0;
    public int menuHSFour = 0;
    public int menuHSFive = 0;

    public SaverScript previousData = new SaverScript();

    private string currentMusic = "";

    public GameObject brickExplosion;

    public float menuBrickExplosionDelayMin = 1.0f;
    public float menuBrickExplosionDelayMax = 3.0f;
    private float menuBrickExplosionTimer = 0.0f;
    private bool brickReadyToExplode = false;
    private float nextBrickExplosionIn;
    public GameObject[] menuBricksArray;
    private int explodedBricksCount;
    private bool bricksRegenerating = false;


    void Awake()
    {

        //lets make sure we don't generate any other menu scripts by accident
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //lets load high scores now if we can
                    }

    void Start()
    {
        menuMusicGenerator.Play();
        currentMusic = "MenuMusic";

        explodedBricksCount = menuBricksArray.Length;
    }

    void FixedUpdate()
    {
        if (!bricksRegenerating)
        {
            BrickExplosionEngine();
        }
    }


    public void BrickExplosionEngine()
    {
        //first lets start our timer for brick exploding
        menuBrickExplosionTimer += Time.fixedDeltaTime;        //time to arm a brick
        if (!brickReadyToExplode)
        {
            brickReadyToExplode = true;
            nextBrickExplosionIn = Random.Range(menuBrickExplosionDelayMin, menuBrickExplosionDelayMax);
        }

        //the time has come to cull a brick
        if (menuBrickExplosionTimer > nextBrickExplosionIn)
        {
            //first lets pick a brick
            GameObject chosenOne = BrickSelectionEngine() as GameObject;

         Instantiate(brickExplosion, chosenOne.transform.position, chosenOne.transform.rotation);
         chosenOne.SetActive(false);
         brickReadyToExplode = false;
         explodedBricksCount--;
         menuBrickExplosionTimer = 0.0f;
        }

        if(explodedBricksCount <= 0 && bricksRegenerating == false)
        {
            bricksRegenerating = true;
            Invoke("RegenerateMenuBricks", 3.0f);
           
        }

    }

    public void RegenerateMenuBricks()
    {
        for (int i = 0; i < menuBricksArray.Length; i++)
        {
            menuBricksArray[i].SetActive(true);
            explodedBricksCount = menuBricksArray.Length;
        }
        bricksRegenerating = false;
    }

    public GameObject BrickSelectionEngine()
    {
        int searchAttempts = 0;
        Search:

                int brickArrayIndex = Random.Range(0, menuBricksArray.Length);
                if(menuBricksArray[brickArrayIndex].gameObject.activeSelf == true)
                {
                    return (menuBricksArray[brickArrayIndex]);
                }
                else if (menuBricksArray[brickArrayIndex].gameObject.activeSelf == false)
                {
                    if(searchAttempts < 100){
                        goto Search;
                    }
                    else
                        Debug.Log("unable to find a brick after 100 attempts!");
                        return(null);
                    
                }
                
                Debug.Log("Error in brick selection engine");
                return (null);
    }




    public void CampaignModeClicked()
    {
        MenuAudioFeedback();
        //campaign mode starts at level 1 
        //must survive until level 5!
        ModeScript.GameMode = "Campaign";
        Debug.Log("Loading Level 1 - campaign mode");
        Debug.Log(ModeScript.GameMode);
        Application.LoadLevel("Level 1");
    }

    public void ArcadeModeClicked()
    {
        MenuAudioFeedback();
        
        ToggleUILevel1();
        ToggleUILevel2();

        
    }

    public void HighScoresClicked()
    {
        //Debug.Log("yay");
        MenuAudioFeedback();

        previousData.LoadGame();
        ToggleUILevel1();
        ToggleUILevel3();
        ToggleIntScoreValue();

        
    }

    public void CreditsClicked()
    {
        //Debug.Log(Application.persistentDataPath);
      //  Debug.Log("switching to credits mode");

        //lets see if we need to enable the credits itself

        MenuAudioFeedback();
        CreditsMusicMode();
        

        if (creditsAnimator.GetBool("CreditsOpened") == false)
        {
            creditsAnimator.SetBool("CreditsOpened", true);
            ToggleUILevel1CreditsMode();
            blurbAnimator.SetBool("BlurbEnabled", true);
            obscuringAnimator.SetBool("ObscuringPanelEnabled", true);
            
            //obscuringPanel.SetActive(true);
            

        }
        else 
        {
            creditsAnimator.SetBool("CreditsOpened", false);
            blurbAnimator.SetBool("BlurbEnabled", false);
            //obscuringPanel.SetActive(false);

            Invoke("ToggleUILevel1CreditsMode", 1f);
            obscuringAnimator.SetBool("ObscuringPanelEnabled", false);
            
            
        }


        
    }

    public void TipsClicked()
    {
        if(tipsAnimator.GetBool("TipsOpen") == false)
        {
            ToggleUILevel1TipsMode();
            tipsAnimator.SetBool("TipsOpen", true);
            tipsBlurbAnimator.SetBool("TipsTextEnabled", true);
            //obscuringPanel.SetActive(true);
            obscuringAnimator.SetBool("ObscuringPanelEnabled", true);
        }
        else 
        {
            ToggleUILevel1TipsMode();
            tipsAnimator.SetBool("TipsOpen", false);
            tipsBlurbAnimator.SetBool("TipsTextEnabled", false);
            //obscuringPanel.SetActive(false);
            obscuringAnimator.SetBool("ObscuringPanelEnabled", false);
        }
    }

    public void ArcadeLevel1Clicked()
    {
        //Arcade Mode bitches!
                ModeScript.GameMode = "Arcade";
        Debug.Log("Loading Level 1 - arcade mode");
        Debug.Log(ModeScript.GameMode);
        Application.LoadLevel("Level 1");
    }

    public void ArcadeLevel2Clicked()
    {
        //Arcade Mode bitches!
        ModeScript.GameMode = "Arcade";
        Debug.Log("Loading Level 2 - arcade mode");
        Debug.Log(ModeScript.GameMode);
        Application.LoadLevel("Level 2");
    }

    public void ArcadeLevel3Clicked()
    {
        //Arcade Mode bitches!
        ModeScript.GameMode = "Arcade";
        Debug.Log("Loading Level 3 - arcade mode");
        Debug.Log(ModeScript.GameMode);
        Application.LoadLevel("Level 3");
    }

    public void ArcadeLevel4Clicked()
    {
        //Arcade Mode bitches!
        ModeScript.GameMode = "Arcade";
        Debug.Log("Loading Level 4 - arcade mode");
        Debug.Log(ModeScript.GameMode);
        Application.LoadLevel("Level 4");
    }

    public void ArcadeLevel5Clicked()
    {
        //Arcade Mode bitches!
        ModeScript.GameMode = "Arcade";
        Debug.Log("Loading Level 5 - arcade mode");
        Debug.Log(ModeScript.GameMode);
        Application.LoadLevel("Level 5");
    }



    public void MainMenuClicked()
    {
        MenuAudioFeedback();
        ToggleUILevel2();
        ToggleUILevel1();
    }

    public void MainMenu2Clicked()
    {
        MenuAudioFeedback();
        ToggleUILevel3();
        ToggleUILevel1();
        ToggleIntScoreValue();
    }

    public void ToggleUILevel1CreditsMode() //this ghetto looking code will toggle our first level UI being on or off
    {
        if (CreditsModeEnabled == false)
        {
            CreditsModeEnabled = true;
            creditsBlurb.SetActive(true);
            obscuringPanel.SetActive(true);
            for (int i = 0; i < UILevel1CreditsMode.Length; i++)
            {
                UILevel1CreditsMode[i].SetActive(false);
                
                //   Debug.Log("turning off: " + UILevel1[i].gameObject);
            }
        }
        else
        {
            CreditsModeEnabled = false;
            creditsBlurb.SetActive(false);
            obscuringPanel.SetActive(false);
            for (int i = 0; i < UILevel1CreditsMode.Length; i++)
            {
                UILevel1CreditsMode[i].SetActive(true);
            }
        }
    }

    public void ToggleUILevel1TipsMode() //this ghetto looking code will toggle our first level UI being on or off
    {
        if (tipsModeEnabled == false)
        {
            tipsModeEnabled = true;
            tipsBlurb.SetActive(true);
            obscuringPanel.SetActive(true);
            for (int i = 0; i < UILevel1TipsMode.Length; i++)
            {
                UILevel1TipsMode[i].SetActive(false);

                //   Debug.Log("turning off: " + UILevel1[i].gameObject);
            }
        }
        else
        {
            tipsModeEnabled = false;
            tipsBlurb.SetActive(false);
            obscuringPanel.SetActive(false);
            for (int i = 0; i < UILevel1CreditsMode.Length; i++)
            {
                UILevel1TipsMode[i].SetActive(true);
            }
        }
    }


    public void ToggleUILevel1() //this ghetto looking code will toggle our first level UI being on or off
    {
        if (UILevel1Enabled == true)
        {
            UILevel1Enabled = false;
            for (int i = 0; i < UILevel1.Length; i++)
            {
                UILevel1[i].SetActive(false);
             //   Debug.Log("turning off: " + UILevel1[i].gameObject);
            }
        }
        else
        {
            UILevel1Enabled = true;
                        for (int i = 0; i < UILevel1.Length; i++)
            {
                UILevel1[i].SetActive(true);
            }
        }
        }

    public void ToggleUILevel2() //this ghetto looking code will toggle our second level UI being on or off
    {
        if (UILevel2Enabled == true)
        {
            UILevel2Enabled = false;
            for (int i = 0; i < UILevel2.Length; i++)
            {
                UILevel2[i].SetActive(false);
             //   Debug.Log("turning off: " + UILevel2[i].gameObject);
            }
        }
        else
        {
            UILevel2Enabled = true;
            for (int i = 0; i < UILevel2.Length; i++)
            {
                UILevel2[i].SetActive(true);
            }
        }
    }

    public void ToggleUILevel3() //this ghetto looking code will toggle our second level UI being on or off
    {
        if (UILevel3Enabled == true)
        {
            UILevel3Enabled = false;
            for (int i = 0; i < UILevel3.Length; i++)
            {
                UILevel3[i].SetActive(false);
          //      Debug.Log("turning off: " + UILevel3[i].gameObject);
            }
        }
        else
        {
            UILevel3Enabled = true;
            for (int i = 0; i < UILevel3.Length; i++)
            {
                UILevel3[i].SetActive(true);
             //   Debug.Log("Turning on: " + UILevel3[i]);
                
                
                

                

            }
        }
    }

    public void ToggleIntScoreValue() //this ghetto looking code will toggle our second level UI being on or off
    {
        if (intScoreValueEnabled == true)
        {
            intScoreValueEnabled = false;
            for (int i = 0; i < intScoreValue.Length; i++)
            {
                intScoreValue[i].gameObject.SetActive(false);
                Debug.Log("turning off: " + intScoreValue[i].gameObject);
            }
        }
        else
        {
            intScoreValueEnabled = true;
            for (int i = 0; i < intScoreValue.Length; i++)
            {
                intScoreValue[i].gameObject.SetActive(true);
                Debug.Log("Turning on: " + intScoreValue[i]);

                intScoreValue[i].text = "" + previousData.ReturnHighScoreI(i);




            }
        }
    }

    public void MenuAudioFeedback() // this will play a boop sound whenever a main menu button is pressed
    {
        float volume = Random.Range(0.4f, 0.8f);

        Debug.Log("Playing a sound for clicking a button!");
       menuButtonSoundGenerator.PlayOneShot(menuButtonPressed, volume);
    }

    public void CreditsMusicMode()
    {
        if(currentMusic =="MenuMusic")
        {
            //while( menuMusicGenerator.volume > 0f)
            //{
            //    menuMusicGenerator.volume -= 0.01f * Time.deltaTime;
             
            //}

            menuMusicGenerator.Pause();
            currentMusic = "CreditsMusic";

            if(!creditsMusicGenerator.isPlaying)
            {
                creditsMusicGenerator.volume = 0.05f;
                creditsMusicGenerator.loop = true;
                creditsMusicGenerator.Play();
                
                //while(creditsMusicGenerator.volume < 1.0f)
                //{
                //    creditsMusicGenerator.volume += 0.01f * Time.deltaTime;
                
                //}
            }
        }
        else if(currentMusic =="CreditsMusic")
        {
            //while (creditsMusicGenerator.volume > 0f)
            //{
            //    creditsMusicGenerator.volume -= 0.01f * Time.deltaTime;
                
            //}

            creditsMusicGenerator.Stop();
            currentMusic = "MenuMusic";

            if (!menuMusicGenerator.isPlaying)
            {
                menuMusicGenerator.volume = 0.05f;
                menuMusicGenerator.loop = true;
                menuMusicGenerator.Play();

                //while (menuMusicGenerator.volume < 1.0f)
                //{
                //    menuMusicGenerator.volume += 0.01f * Time.deltaTime;
          
                //}
            }
        }
        else
        {
            Debug.Log("menu switching error! CreditsMusicMode Function!!!");
        }





    }
}


