using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    WHEEL,
    BODY,
    CHASSIS,
    MOTOR,
    BUMPER,
    COSMETIC
}

public class CarPart : MonoBehaviour {

    public PartType type; //What type of part this is.
    public GameObject mesh; //The mesh to represent the object.
    public TextAsset stats; //Read a file with stats.  Each part type takes in a different combination of values.
    public CustomCarController car; //the car we are editing

    [Header("General")]
    public string partName; //the name
    public float mass; //mass.  functionality depends on what part it is.

    [Header("Wheel")]
    public float dampRate; //wheelDampingRate
    public float suspensionDist; //suspensionDistance
    public float FAPD; //forceAppPointDistance
    public float fStiff; //forwardFriction.stiffness
    public float sStiff; //sidewaysFriction.stiffness

    //Used by car to see if all parts are initialized.
    public bool isInitialized { get { return _isInitialized; } }
    bool _isInitialized = false;

    // Use this for initialization
    private void Awake()
    {
     switch(type)
        {
            case PartType.WHEEL:
                ParseWheel(); break;
            case PartType.BODY:
                ParseBody(); break;
            case PartType.CHASSIS:
                ParseChassis(); break;
            case PartType.MOTOR:
                ParseMotor(); break;
            case PartType.BUMPER:
                ParseBumper(); break;
            case PartType.COSMETIC:
                ParseCosmetic(); break;
            default:
                break;
        }
        _isInitialized = true;
    }

    //TODO: combine these into a single Parse() function

    void ParseWheel()
    {

    }
    void ParseBody()
    {

    }
    void ParseChassis()
    {

    }
    void ParseMotor()
    {

    }
    void ParseBumper()
    {

    }
    void ParseCosmetic()
    {

    }
}
