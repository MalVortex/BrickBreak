using UnityEngine;
using System.Collections;

public class ModeScript : MonoBehaviour {

    public static string GameMode = "null";
    public static ModeScript instance = null;

    // modes are "Arcade", "Campaign"


    void Awake()
    {

        if(instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }


        
    }

}
