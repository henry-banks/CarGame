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
    public float impulseStrength = 10f; //force of impulses like ROCKETS
    public Vector3 impulse; //used to push the car with a single forceful impulse
    public float groundDistThreshold = 0.1f; //used to check if we are touching the ground

    //Properties

    //https://answers.unity.com/questions/17968/finding-the-bounds-of-a-grouped-model.html
    public float distToGround { get {
            Bounds combinedBounds;
            var renderers = GetComponentsInChildren<Renderer>();
            combinedBounds = renderers[0].bounds;
            foreach (Renderer render in renderers)
            {
                //Skip the first
                if(render != renderers[0])combinedBounds.Encapsulate(render.bounds);
            }
            return combinedBounds.extents.y;
        } }
    public Rigidbody rb;
    bool canJump;

    public void Start()
    {
        //Initialize component variables
        rb = GetComponent<Rigidbody>();

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

    private void Update()
    {
        float jump = Input.GetAxis("Jump");

        //Reset canJump if the jump button is depressed.
        if (jump <= 0 && !canJump) canJump = true;
    }

    private void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float handbrake = Input.GetAxis("Brake");
        float jump = Input.GetAxis("Jump");

        BrakeInput(handbrake, ref motor);
        JumpInput(jump);

        WheelInput(motor, steering);
    }

    //Update functions
    void BrakeInput(float handbrake, ref float motor)
    {
        if (handbrake > 0)
        {
            motor = 0;
            GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, 0.2f);
        }
    }
    void JumpInput(float jump)
    {
        if(IsOnGround() && jump > 0 && canJump)
        {
            canJump = false;
            impulse = new Vector3(0, impulseStrength, 0);
            ApplyImpulse();
        }
    }
    void WheelInput(float motor, float steering)
    {
        foreach (AxleInfo a in axels)
        {
            if (a.steering)
            {
                a.leftWheel.steerAngle = steering;
                a.rightWheel.steerAngle = steering;
            }
            if (a.motor && IsOnGround())
            {
                a.leftWheel.motorTorque = motor;
                a.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(a.leftWheel);
            ApplyLocalPositionToVisuals(a.rightWheel);
        }
    }

    //Returns true if we are on the ground
    bool IsOnGround()
    {   return Physics.Raycast(transform.position, -Vector3.up, distToGround + groundDistThreshold);    }

    //Applies the current impulse to the car and resets it
    void ApplyImpulse()
    {
        rb.AddForce(impulse, ForceMode.Impulse);
        impulse = Vector3.zero;
        Debug.Log("Jump!");
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