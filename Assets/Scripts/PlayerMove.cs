using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
public class PlayerMove : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] float speed = 5f;
    Rigidbody rb;
    Vector3 direction;
    Animator animator;
    [SerializeField] float JumpForce = 7f;
    bool isGrounded = true;
    [SerializeField] GameObject car;
    Car carController;
    [SerializeField] Transform waypoint;
    [SerializeField] Camera carCamera;  
    [SerializeField] float radius;       
    bool isDriver = false;               
    NavMeshAgent agent;
    [SerializeField] Button exitCarButton; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        carController = car.GetComponent<Car>();
        agent = GetComponent<NavMeshAgent>();

        
        if (exitCarButton != null)
        {
            exitCarButton.onClick.AddListener(ExitCar); 
        }
    }

    void Update()
    {
        if (!isDriver)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            direction = transform.TransformDirection(horizontal, 0, vertical);
            animator.SetFloat("move", Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)));
        }

        if (isDriver && agent.remainingDistance < 0.25f)
        {
            SwitchCamera();
            isDriver = true;
            agent.enabled = false;
            transform.LookAt(car.transform);
            carController.enabled = true;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + speed * direction);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("jump", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    public void goTocar()
    {
        StartCoroutine(inCar());
    }

    private void SwitchCamera()
    {
        carCamera.enabled = true;
        gameObject.SetActive(false);
        gameObject.transform.SetParent(car.transform);
    }

    IEnumerator inCar()
    {
        if (Vector3.Distance(transform.position, car.transform.position) <= radius && !isDriver)
        {
            agent.enabled = true;
            agent.SetDestination(waypoint.position);
            yield return new WaitForSeconds(1);
            isDriver = true;
            animator.SetFloat("move", 1f);
        }
    }

   
    public void ExitCar()
    {
      
        carController.enabled = false;          
        transform.position = car.transform.position + new Vector3(2f, 0, 2f); 

        
        carCamera.enabled = false;
        gameObject.SetActive(true);             
        agent.enabled = false;                  

        gameObject.transform.SetParent(null);  
        isDriver = false;                     
    }
}
