using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all exploding thingos.  pretty simple atm.
public class Exploder : MonoBehaviour {

    public GameObject explosion;

	public void Explode()
    {
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
	}
}
