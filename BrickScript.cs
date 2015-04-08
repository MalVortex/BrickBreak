using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

    public GameObject brickParticle;
    public GameObject brickStillPersists;

    public int hitpoints = 1;
    public int scoreValue = 0;

    private int redBrickValue = 5;
    private int greyBrickValue = 10;
    private int acceleratorBrickValue = 15;
    private int spawnerBrickValue = 30;

    public Color damagedColor;

    void Start()
    {
        //rather than changing every brick score point in every prefab, lets see if we can do it by script
        string brickName;
        brickName = this.gameObject.name;

        switch (brickName)
        {
            case "Cube":
                scoreValue = redBrickValue;
                break;
            case "GreyBrick":
                scoreValue = greyBrickValue;
                break;
            case "Accelerator":
                scoreValue = acceleratorBrickValue;
                break;
            case "Spawner":
                scoreValue = spawnerBrickValue;
                break;
            default:
                Debug.Log("WTF cant find a brick!");
                break;


        }

    }

    

    void OnCollisionEnter(Collision other)
    {

        hitpoints--;
        
        if(gameObject.name=="Accelerator")
        {
            //Debug.Log("Ball Velocity: " + other.rigidbody.velocity);
            other.rigidbody.velocity = other.rigidbody.velocity * 1.2f;
            //Debug.Log("After Boost, Ball Velocity is now: " + other.rigidbody.velocity);
        }
        else if(gameObject.name=="Spawner")
        {
            GameScript.instance.MoreBalls();
        }

        if(hitpoints <= 0)
        {     
        Instantiate(brickParticle, transform.position, Quaternion.identity);
        GameScript.instance.DestroyBrick(scoreValue);
        Destroy(gameObject);
        }
        else 
        {
            Instantiate(brickStillPersists, transform.position, Quaternion.identity);
            GetComponent<Renderer>().material.color = damagedColor;
        }
    }


}
