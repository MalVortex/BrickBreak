using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallScript : MonoBehaviour {

    

	public float ballImpulseHorizontal = 100f;
	public float ballImpulseVertical = 800f;

    public AudioClip bounce;

    private Rigidbody ballBody;
    private bool ballInPlay = false;
    

    void Awake()
    {
        //boosterCurretCharge = 0f;
        ballBody = GetComponent<Rigidbody>();
     }

	// Use this for initialization
	void Start () {

	
	}

    void fixedUpdate()
    {
        //this prevents the game from freezing if something weird happens
        //its ugly and should probably be improved at some point
        
        
    }

    void OnCollisionEnter(Collision other)
    {
        AudioSource.PlayClipAtPoint(bounce, transform.position);
        
    }
	
	// Update is called once per frame
	void Update ()  {
        if (Input.GetButtonDown("Jump") && ballInPlay == false)
        {
            ballInPlay = true;
            GameScript.instance.ballIsLive = true;
            transform.parent = null;
            ballBody.isKinematic = false;
            ballBody.AddForce(new Vector3(ballImpulseHorizontal, ballImpulseVertical, 0f));
        }

        
	}

}
