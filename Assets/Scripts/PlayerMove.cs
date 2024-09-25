using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] float speed = 5;
    Rigidbody rb;
    Vector3 direction;
    Animator animator;
    [SerializeField] float JumpForce = 7f;
    bool isGrounded = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        direction = transform.TransformDirection(horizontal, 0, vertical);
        animator.SetFloat("move", Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)));
        
        

    }
     void FixedUpdate()
    {
        rb.MovePosition(transform.position + speed * direction);
    }
    public void Jump()
    {
        if  (isGrounded)
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
}

