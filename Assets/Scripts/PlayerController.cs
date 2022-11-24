using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private Gun gun;

    private Rigidbody _rb;
    private Vector3 movement = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded())
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            
        }

        if (Input.GetButton("Fire1"))
        {
            gun.Fire();
        }
    }

    void FixedUpdate()
    {
        // Lateral Movement
        movement = Vector3.zero;
        movement += transform.forward * Input.GetAxis("Vertical") * _speed;
        movement += transform.right * Input.GetAxis("Horizontal") * _speed;
        movement = Vector3.ClampMagnitude(movement, _speed);
        _rb.velocity = movement;

        // Gravity
        _rb.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);
    }
}
