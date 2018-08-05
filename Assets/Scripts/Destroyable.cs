using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A car part that can be damaged and destroyed.

//https://answers.unity.com/questions/767962/want-velocity-along-a-direction.html
//Velocity calculation help.
public class Destroyable : MonoBehaviour {


    public float health;
    //How fast the object has to be going in a direction before exploding.
    public float hitThreshold;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > hitThreshold)
    }
}
