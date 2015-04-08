using UnityEngine;
using System.Collections;

public class DangerZone : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Ball")
        {
            GameScript.instance.LoseLife();
            Debug.Log("Ball destroyed by water hazard");
        }
        else
        {
            Debug.Log("Water Hazard: Destroying object: " + other.name);
            Destroy(other, 5f);

        }
        
    }


}
