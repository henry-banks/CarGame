using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://docs.unity3d.com/Manual/WheelColliderTutorial.html

public class CustomCarController : MonoBehaviour {

    public List<AxleInfo> axels; //info about each axel
    List<WheelCollider> wheelColliders = new List<WheelCollider>();

    [Header("Car Parts")]
    public CarPart body;
    public CarPart chassis;
    public CarPart wheels;
    public CarPart motor;
    public CarPart bumper;
    public List<CarPart> cosmetics;

    List<CarPart> allParts = new List<CarPart>();

    public float speedMult = 1; //SPEED MULTIPLY
    public float maxMotorTorque; //max torque the motor can apply
    public float maxSteeringAngle; //maximum steer angle
    public float brakeForce = 100; //brake force
    public float brakeBias = .8f; //lerp alpha

    public void Start()
    {
        //Is there a better way to do this?
        allParts.Add(body);
        allParts.Add(chassis);
        allParts.Add(wheels);
        allParts.Add(motor);
        allParts.Add(bumper);
        foreach (CarPart c in cosmetics)
            allParts.Add(c);

        foreach(AxleInfo a in axels)
        {
            wheelColliders.Add(a.leftWheel);
            wheelColliders.Add(a.rightWheel);
        }

        BuildCar();
        //StartCoroutine(BuildCarDelay());
    }

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
        visualWheel.transform.Rotate(0, 0, 90);
    }

    private void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float handbrake = Input.GetAxis("Jump");

        if (handbrake > 0)
        {
            motor = 0;
            GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, 0.2f);
        }

        foreach (AxleInfo a in axels)
        {
            if (a.steering)
            {
                a.leftWheel.steerAngle = steering;
                a.rightWheel.steerAngle = steering;
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

    //Delay the building of the car so that the car parts are initialized.
    IEnumerator BuildCarDelay()
    {
        foreach(CarPart c in allParts)
        {
            if(c)
            {
                //Make sure all parts are initialized
                while(!c.isInitialized)
                    yield return new WaitForSeconds(0.1f);
            }
        }

        BuildCar();
    }

    //TEST: change wheel values
    void BuildCar()
    {
        //Wheels
        if(wheels)
        {
            foreach(WheelCollider w in wheelColliders)
            {
                w.mass = wheels.mass;
                w.wheelDampingRate = wheels.dampRate;
                w.suspensionDistance = wheels.suspensionDist;
                w.forceAppPointDistance = wheels.FAPD;
            
                WheelFrictionCurve temp = w.forwardFriction;
                temp.stiffness = wheels.fStiff;
                w.forwardFriction = temp;
                temp = w.sidewaysFriction;
                temp.stiffness = wheels.sStiff;
                w.sidewaysFriction = temp;

                Instantiate(wheels.mesh, w.transform);
            }
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