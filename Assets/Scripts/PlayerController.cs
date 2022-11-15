using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private GroundCheck groundCheck;

    private Rigidbody _rb;
    private Vector3 _movement = Vector2.zero;

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
       
    }

    void FixedUpdate()
    {
        _movement = Vector3.zero;
        _movement += transform.forward * Input.GetAxis("Vertical") * _speed;
        _movement += transform.right * Input.GetAxis("Horizontal") * _speed;
        _movement = Vector3.ClampMagnitude(_movement, _speed);
        _rb.velocity = _movement;
        _rb.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);




        
    }


}
