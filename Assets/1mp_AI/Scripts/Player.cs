using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
public class Player : MonoBehaviour
{
    [SerializeField] Joystick Drive;
    [SerializeField] float motorSpeed;
    [SerializeField] float MaxWheel;
    bool LightsOn;
    [SerializeField]Light left, right;
    
    public Car car { get; private set; }
    private void Awake()
    {
        car = GetComponent<Car>();
        left.enabled = LightsOn;
        right.enabled = LightsOn;

    }
    public void Lights()
    {
        LightsOn = !LightsOn;
        left.enabled = LightsOn;
        right.enabled = LightsOn;
    }
    void Update()
    {
       
        car.motor = motorSpeed * Drive.Vertical;
        car.Angle = Drive.Horizontal * MaxWheel;
    }
}