using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 50;
    float rotationSpeed = 100;

    public float moveX;
    public float moveY;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);

        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            transform.Translate(0, 0, vertical * speed/2 * Time.deltaTime);
        else
            transform.Translate(0, 0, vertical * speed * Time.deltaTime);

        moveX = horizontal * speed * Time.deltaTime;
        moveY = vertical * speed * Time.deltaTime;
    }
}
