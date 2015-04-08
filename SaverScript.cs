using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaverScript
{

        public float testNumberPleaseIgnore = 37;

        //private data

        //Arcade Access
        int levelsUnlocked = 1;

        //Adventure Mode Data
        string adventurerName = "debug";
        float adventureTime = -1f;


        //Arcade score data
        int levelOneHighScore = -1;
        string levelOneHighName = "debug";

        int levelTwoHighScore = -2;
        string levelTwoHighName = "debug";

        int levelThreeHighScore = -3;
        string levelThreeHighName = "debug";

        int levelFourHighScore = -4;
        string levelFourHighName = "debug";

        int levelFiveHighScore = -5;
        string levelFiveHighName = "debug";

        //constructor


        //default. no arguments passed, creates game data class with no progress and blank values
        public SaverScript()
        {
            levelsUnlocked = 1;

            adventurerName = "N/A";
            adventureTime = 0.0f;


            levelOneHighScore = 0;
            levelOneHighName = "N/A";

            levelTwoHighScore = 0;
            levelTwoHighName = "N/A";

            levelThreeHighScore = 0;
            levelThreeHighName = "N/A";

            levelFourHighScore = 0;
            levelFourHighName = "N/A";

            levelFiveHighScore = 0;
            levelFiveHighName = "N/A";
        }



        //mk1 constructor. initializes unlocked levels, and no high scores
        public SaverScript(int levelsToUnlock)
        {
            levelsUnlocked = levelsToUnlock;

            adventurerName = "N/A";
            adventureTime = 0.0f;


            levelOneHighScore = 0;
            levelOneHighName = "N/A";

            levelTwoHighScore = 0;
            levelTwoHighName = "N/A";

            levelThreeHighScore = 0;
            levelThreeHighName = "N/A";

            levelFourHighScore = 0;
            levelFourHighName = "N/A";

            levelFiveHighScore = 0;
            levelFiveHighName = "N/A";
        }

        //mk2 constructuor. initializes everything with passed values

        public SaverScript(int levelsToUnlock, float timeToCompleteAdventure, string theAdventurersName,
                            int scoreForLevelOne, string guyForLevelOne,
                            int scoreForLevelTwo, string guyForLevelTwo,
                            int scoreForLevelThree, string guyForLevelThree,
                            int scoreForLevelFour, string guyForLevelFour,
                            int scoreForLevelFive, string guyForLevelFive)
        {
            levelsUnlocked = levelsToUnlock;

            adventureTime = timeToCompleteAdventure;
            adventurerName = theAdventurersName;

            levelOneHighScore = scoreForLevelOne;
            levelOneHighName = guyForLevelOne;

            levelTwoHighScore = scoreForLevelTwo;
            levelTwoHighName = guyForLevelTwo;

            levelThreeHighScore = scoreForLevelThree;
            levelThreeHighName = guyForLevelThree;

            levelFourHighScore = scoreForLevelFour;
            levelFourHighName = guyForLevelFour;

            levelFiveHighScore = scoreForLevelFive;
            levelFiveHighName = guyForLevelFive;


        }




        //get-set methods

        public int unlockedLevels
        {
            get
            {
                return levelsUnlocked;
            }
            set
            {
                levelsUnlocked = value;
            }
        }

        public float adventuringTime
        {
            get
            {
                return adventureTime;
            }
            set
            {
                adventureTime = value;
            }
        }

        public string adventuringName
        {
            get
            {
                return adventurerName;
            }
            set
            {
                adventurerName = value;

            }
        }

        public int highScoreOne
        {
            get
            {
                return levelOneHighScore;
            }
            set
            {
                levelOneHighScore = value;
            }
        }

        public string highNameOne
        {
            get
            {
                return levelOneHighName;
            }
            set
            {
                levelOneHighName = value;
            }
        }

        public int highScoreTwo
        {
            get
            {
                return levelTwoHighScore;
            }
            set
            {
                levelTwoHighScore = value;
            }
        }

        public string highNameTwo
        {
            get
            {
                return levelTwoHighName;
            }
            set
            {
                levelTwoHighName = value;
            }
        }

        public int highScoreThree
        {
            get
            {
                return levelThreeHighScore;
            }
            set
            {
                levelThreeHighScore = value;
            }
        }

        public string highNameThree
        {
            get
            {
                return levelThreeHighName;
            }
            set
            {
                levelThreeHighName = value;
            }
        }

        public int highScoreFour
        {
            get
            {
                return levelFourHighScore;
            }
            set
            {
                levelFourHighScore = value;
            }
        }

        public string highNameFour
        {
            get
            {
                return levelFourHighName;
            }
            set
            {
                levelFourHighName = value;
            }
        }

        public int highScoreFive
        {
            get
            {
                return levelFiveHighScore;
            }
            set
            {
                levelFiveHighScore = value;
            }
        }

        public string highNameFive
        {
            get
            {
                return levelFiveHighName;
            }
            set
            {
                levelFiveHighName = value;
            }
        }

    public int ReturnHighScoreI(int i)
    {
        switch (i)
        {
            case 0:
                return (levelOneHighScore);
            case 1:
                return (levelTwoHighScore);
            case 2:
                return (levelThreeHighScore);
            case 3:
                return (levelFourHighScore);
            case 4:
                return (levelFiveHighScore);
            default:
                return(-50);
        }


    }
     


        //-------------------------------------------------------------------------

        //save-load functionality

        public void SaveGame(int highScoreOne, int highScoreTwo, int highScoreThree, int highScoreFour, int highScoreFive)
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/BrickBreak.dat");

            SaverScript saver = new SaverScript();

            saver.levelOneHighScore = highScoreOne;
            saver.levelTwoHighScore = highScoreTwo;
            saver.levelThreeHighScore = highScoreThree;
            saver.levelFourHighScore = highScoreFour;
            saver.levelFiveHighScore = highScoreFive;




            bf.Serialize(file, saver);
            file.Close();


        }

        public void LoadGame()
        {
            if (File.Exists(Application.persistentDataPath + "/BrickBreak.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/BrickBreak.dat", FileMode.Open);
                SaverScript savedData = (SaverScript)bf.Deserialize(file);


                levelOneHighScore = savedData.levelOneHighScore;
                levelTwoHighScore = savedData.levelTwoHighScore;
                levelThreeHighScore = savedData.levelThreeHighScore;
                levelFourHighScore = savedData.levelFourHighScore;
                levelFiveHighScore = savedData.levelFiveHighScore;

                file.Close();


            }
            else
            {
                //there is no save game file so lets create a new one to work with
                SaveGame(0, 0, 0, 0, 0);

            }
        }




    }














