using UnityEngine;
using System.Collections;

public class BallOutofBounds : MonoBehaviour 

{

    void OnTriggerExit(Collider other)
    {

        if( other.name == "Ball")
        {
            GameScript.instance.LoseLife();
            Debug.Log("Ball destroyed by out of bounds");
        }
        else if (other.name == "Wall")
        {
            Debug.Log("why is this wall out of bounds?????");
            
        }
        else
        {
            Debug.Log("Out of Bounds: Destroying object: " + other.name);
            Destroy(other, 5f);

        }
        


    }



}
