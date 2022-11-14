using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cam;

    private Rigidbody _rb;
    private Vector2 _mouse = Vector2.zero;
    private Vector3 _movement = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        _mouse.x += Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        _mouse.y -= Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
        _mouse.y = Mathf.Clamp(_mouse.y, -90f, 90f);
        transform.localRotation = Quaternion.Euler(0, _mouse.x, 0);
        _cam.localRotation = Quaternion.Euler(_mouse.y, 0, 0);

        _movement = Vector3.zero;
        _movement += transform.forward * Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        _movement += transform.right * Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        _movement = Vector3.ClampMagnitude(_movement, _speed);
    }

    void FixedUpdate()
    {
        _rb.velocity = _movement;
    }
}
