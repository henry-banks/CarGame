using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A car part that can be damaged and destroyed.

//https://answers.unity.com/questions/767962/want-velocity-along-a-direction.html
//Velocity calculation help.
public class Destroyable : MonoBehaviour {

    public GameObject explosion;
    public float health;
    //How fast the object has to be going in a direction before exploding.
    public float hitThreshold;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name + " " + collision.relativeVelocity.magnitude);
        if(collision.relativeVelocity.magnitude > hitThreshold)
        {
            //Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            //Destroy(gameObject);
        }
    }
}
