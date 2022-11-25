using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundMaxSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airMaxSpeed;
    [SerializeField] private float airDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private Gun gun;


    private enum State
    {
        Grounded,
        Air
    }

    private State state;
    private Rigidbody rb;
    private Vector3 input = Vector2.zero;
    private float jumpTimer;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        state = State.Air;
    }


    void Update()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (state == State.Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                state = State.Air;
                rb.drag = airDrag;
                jumpTimer = 0.5f;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
            
            if (!groundCheck.IsGrounded())
            {
                state = State.Air;
                rb.drag = airDrag;
            }
        }

        if (state == State.Air)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0 && groundCheck.IsGrounded())
            {
                state = State.Grounded;
                rb.drag = groundDrag;
            }
        }

        if (Input.GetButton("Fire1"))
        {
            gun.Fire();
        }
    }

    void FixedUpdate()
    {
        if (state == State.Grounded)
        {
            //rb.velocity = transform.TransformDirection(input) * groundMaxSpeed;
            /*if (direction.magnitude == 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }*/
            if (rb.velocity.magnitude < groundMaxSpeed)
            {
                rb.AddRelativeForce(input * groundAcceleration);
            }
        }

        if (state == State.Air)
        {
            /** Makes it so air strafing is possible but player input can still steer player without breaching max speed */
            if (Mathf.Abs(transform.InverseTransformDirection(rb.velocity).x) > airMaxSpeed)
            {
                if (Mathf.Sign(input.x) == Mathf.Sign(transform.InverseTransformDirection(rb.velocity).x))
                {
                    input.x = 0;
                }
            }

            if (Mathf.Abs(transform.InverseTransformDirection(rb.velocity).z) > airMaxSpeed)
            {
                if (Mathf.Sign(input.z) == Mathf.Sign(transform.InverseTransformDirection(rb.velocity).z))
                {
                    input.z = 0;
                }
            }
            rb.AddRelativeForce(input * airAcceleration);
        }

        // Lateral Movement
        /*movement = Vector3.zero;
        movement += transform.forward * Input.GetAxis("Vertical") * speed;
        movement += transform.right * Input.GetAxis("Horizontal") * speed;
        movement = Vector3.ClampMagnitude(movement, speed);
        rb.velocity = movement;*/

        // Gravity
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }
}
