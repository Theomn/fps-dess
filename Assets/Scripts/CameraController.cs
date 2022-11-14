using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerEyes;

    private Vector2 mouse;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerEyes.position, ref velocity, Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        mouse.x += Input.GetAxis("Mouse X") * sensitivity;
        mouse.y -= Input.GetAxis("Mouse Y") * sensitivity;
        mouse.y = Mathf.Clamp(mouse.y, -90f, 90f);
        player.localRotation = Quaternion.Euler(0, mouse.x, 0);
        transform.localRotation = Quaternion.Euler(mouse.y, mouse.x, 0);
    }
}
