using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoosterScript : MonoBehaviour {

    public Image chronoImage;

    public float currentCD;
    public float maxCD;

  

	// Use this for initialization
	void Start () {

        

        chronoImage = GetComponent<Image>();

	}
	
	// Update is called once per frame
	void Update () 
    {

        currentCD = GameScript.instance.boosterCooldown;
        maxCD = GameScript.instance.boosterBaseCooldown;

        //chronoImage.fillAmount = 0.5f; //tests to see if we are even talking to the thing correctly

        chronoImage.fillAmount = Mathf.Clamp((currentCD / maxCD), 0f, 1f);
        
	
	}
}
