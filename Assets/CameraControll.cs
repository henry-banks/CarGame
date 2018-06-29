using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    PlayerController playerController;
    private Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        playerController = FindObjectOfType<PlayerController>();

        offset = transform.position - playerController.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position = playerController.transform.position + offset;
	}

    void Update()
    {
        transform.rotation = playerController.transform.rotation;
    }
}
