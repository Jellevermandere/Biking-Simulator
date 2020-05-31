using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    [Header ("GameObjects")]
    public GameObject rearWheel;
    public GameObject frontWheel;
    public GameObject crank;
    public GameObject pedalL;
    public GameObject pedalR;
    public GameObject fork;
    public Transform centerOfMass;

    [Header("Values")]
    public float oneRotationSpeed = 2.7f;
    public float crankMultiplier = 2f;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    [Range(0,1)]
    public float relativeLeanAmount = 0f;
    public Transform leftWheels;
    public Transform rightWheels;

    private float rotationValue = 0f;
    private float maxSpeed = 0f;
    public float rotSpeed = 10;

    private Vector3[] wheelPositions;
    private Rigidbody rb;
    private Quaternion startForkRot;
    private Vector3 upDirection = Vector3.up;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        startForkRot = fork.transform.localRotation;
        wheelPositions = new Vector3[axleInfos.Count];
        for (int i = 0; i < axleInfos.Count; i++)
        {
            wheelPositions[i] = axleInfos[i].wheel.center;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateMeshes();
        RotateFork();
        //Debug.Log("rotation: " + rotationValue); 
    }

    public void FixedUpdate()
    {
  
        ApplyWheelForce();
        //RotateStraight();

    }

    void RotateMeshes()
    {
        RotateObject(crank, 1);
        RotateObject(pedalL, -1);
        RotateObject(pedalR, -1);
        RotateObject(rearWheel, crankMultiplier);
        RotateObject(frontWheel, crankMultiplier);
    }

    void RotateFork()
    {
        fork.transform.localRotation = startForkRot;
        fork.transform.RotateAround(fork.transform.position, fork.transform.up, maxSteeringAngle * rotationValue);
    }

    void Lean()
    {
        upDirection = Vector3.Normalize(Vector3.up + transform.right * maxSteeringAngle * relativeLeanAmount * rotationValue* rb.velocity.magnitude / 100);
    }
    

    void ApplyWheelForce()
    {
        float motor = maxMotorTorque;
        float steering = maxSteeringAngle * rotationValue;

        leftWheels.localPosition = - Vector3.up * relativeLeanAmount * rotationValue * rb.velocity.magnitude;
        rightWheels.localPosition = Vector3.up * relativeLeanAmount * rotationValue * rb.velocity.magnitude;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            
            if (axleInfo.steering)
            {
                axleInfo.wheel.steerAngle = steering;
            }
            if (axleInfo.motor && rb.velocity.magnitude < maxSpeed)
            {
                axleInfo.wheel.motorTorque = motor;
            }
            else if(axleInfo.motor)
            {
                axleInfo.wheel.motorTorque = 0;
            }
        }
    }

    public void setRotationAndSpeed(Vector2 value)
    {
        rotationValue = value.x;
        maxSpeed = value.y;
    }
    


    // obsolete
    private void RotateStraight()
    {
        Vector3 axis = Vector3.Cross(transform.forward, Vector3.Cross(upDirection, transform.forward));
        float angle = Vector3.Angle(transform.up, axis);
        Vector3 direction = Vector3.Cross(transform.up, axis);
        if (angle > 0.001)
        {
            rb.AddTorque( direction * angle * 100);
        }
    }
    //rotates the meshes
    void RotateObject(GameObject obj, float multiplier)
    {
        obj.transform.Rotate(Time.deltaTime * rb.velocity.magnitude * (360f / oneRotationSpeed) * multiplier, 0, 0);
        //obj.transform.Rotate(Time.deltaTime * rotSpeed * (360f / oneRotationSpeed) * multiplier, 0, 0);
    }
}



[System.Serializable]
public class AxleInfo
{
    public WheelCollider wheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}

