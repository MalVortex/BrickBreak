using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

	public float paddleSpeed = 10f;
	public float paddleBoost = 100f;

    private Vector3 playerPosition = new Vector3(0, -9f, 0);

	// Use this for initialization
	void Start () {
	
	}

    void FixedUpdate()
    {
        float xPosition = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
        playerPosition = new Vector3(Mathf.Clamp(xPosition, -8f, 8f), -9f, 0);
        transform.position = playerPosition;
    }






    // Update is called once per frame
   

    void OnCollisionEnter(Collision col)
    {
        foreach (ContactPoint contact in col.contacts)
        {
            if (contact.thisCollider == GetComponent<Collider>())
            {
                //this is the paddles contact point
                float booster = contact.point.x - transform.position.x;

                contact.otherCollider.GetComponent<Rigidbody>().AddForce(paddleBoost * booster, 0, 0);
            }
        }
    }

}
	