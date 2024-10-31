using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    [SerializeField] Axis[] CarAxis;
    
    [SerializeField] float BreakForce;
    [SerializeField] Transform COM;
    Rigidbody rb;
    bool isBreak;
    [SerializeField] TrailRenderer leftWheel;
    [SerializeField] TrailRenderer rightWheel;
    AudioSource engineAudio;
    public Joystick joystick;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = COM.localPosition;
        engineAudio = GetComponent<AudioSource>();
        
        
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
    private void FixedUpdate(Joystick joystick)
    {

        float pi = Mathf.Lerp(0.6f, 1.6f, Mathf.Abs(joystick.Vertical));
        GetComponent<AudioSource>().pitch = Mathf.Lerp(GetComponent<AudioSource>().pitch, pi, 0.01f);
    }
    public float motor;
    public float Angle; 
    void Update()
    {
      

       
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
                leftWheel.emitting = true;
                rightWheel.emitting = true;
                axis.Right.brakeTorque = BreakForce;
            }
            else
            {
                axis.Left.brakeTorque = 0;
                leftWheel.emitting = false;
                rightWheel.emitting = false;
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