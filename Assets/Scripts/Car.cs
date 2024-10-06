using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] Axis[] CarAxis;
    [SerializeField] Joystick Drive;
    [SerializeField] float motorSpeed;
    [SerializeField] float MaxWheel;
    [SerializeField] float BreakForce;
    [SerializeField] Transform COM;
    Rigidbody rb;
    bool isBreak;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = COM.localPosition;
    }

    // Update is called once per frame
    public void StopOn()
    {
        isBreak = true;
    }
    public void StopOff()
    {
        isBreak = false;
    }
    public void ApplyLocalPositionToVisuals(Axis collider)
    {        
        Vector3 position;
        Quaternion rotation;
        collider.Left.GetWorldPose(out position, out rotation);
        collider.LeftObj.transform.position = position;
        collider.LeftObj.transform.rotation = rotation;
        collider.Right.GetWorldPose(out position, out rotation);
        collider.RightObj.transform.position = position;
        collider.RightObj.transform.rotation = rotation;
    }
    void Update()
    {
        float motor = motorSpeed * Drive.Vertical;
        float Angle = Drive.Horizontal * MaxWheel;
        foreach ( Axis axis in CarAxis)
        {
            if (axis.steering) 
            { 
                axis.Left.steerAngle = Angle;
                axis.Right.steerAngle = Angle;
            }
            if (axis.motor)
            {
                axis.Left.motorTorque = motor;
                axis.Right.motorTorque = motor;
            }
            if (isBreak)
            {
                axis.Left.brakeTorque = BreakForce;
                axis.Right.brakeTorque = BreakForce;
            }
            else
            {
                axis.Left.brakeTorque = 0;
                axis.Right.brakeTorque = 0;
            }
            ApplyLocalPositionToVisuals(axis);
        }
    }
}
[System.Serializable]
public class Axis
{
    public bool motor;
    public bool steering;
    public WheelCollider Left;
    public WheelCollider Right;
    public GameObject LeftObj;
    public GameObject RightObj;
}