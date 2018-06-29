using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://docs.unity3d.com/Manual/WheelColliderTutorial.html

public class CarController : MonoBehaviour {

    public List<AxleInfo> axels; //info about each axel
    public float maxMotorTorque; //max torque the motor can apply
    public float maxSteeringAngle; //maximum steer angle

    //finds the visual wheel and applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        //If there are no children, return
        if(collider.transform.childCount == 0) { return; }

        //get the first child
        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 pos;
        Quaternion rot;
        //this is cool...
        collider.GetWorldPose(out pos, out rot);

        visualWheel.transform.position = pos;
        visualWheel.transform.rotation = rot;
    }

    private void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach(AxleInfo a in axels)
        {
            if (a.steering)
            {
                a.leftWheel.motorTorque = motor;
                a.rightWheel.motorTorque = motor;
            }
            if (a.motor)
            {
                a.leftWheel.motorTorque = motor;
                a.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(a.leftWheel);
            ApplyLocalPositionToVisuals(a.rightWheel);
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; //Is the wheel attached to a motor?
    public bool steering; //Does this wheel steer?
}