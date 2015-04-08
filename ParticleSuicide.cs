using UnityEngine;
using System.Collections;

public class ParticleSuicide : MonoBehaviour {

    public float deathTimer = 5f;

	// Use this for initialization
	void Start () 
    {
        
        Destroy(gameObject, deathTimer);
	}
	

    
}
