using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomSensitivityMultiplier;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject viewmodelCamera;

    private Vector2 mouse;
    private Vector3 velocity;
    private LayerMask crosshairLayerMask;
    private Transform player;
    private Transform playerEyes;
    private float initialFOV;
    private float targetFOV;
    private bool isZoomed;
    private Camera cam;



    protected override void Awake()
    {
        base.Awake();
        crosshairLayerMask = LayerMask.GetMask("Ground", "Enemy");
        cam = mainCamera.GetComponent<Camera>();
        initialFOV = cam.fieldOfView;
        targetFOV = initialFOV;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = PlayerController.instance.transform;
        playerEyes = PlayerController.instance.GetEyes();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerEyes.position, ref velocity, Time.fixedDeltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    void LateUpdate()
    {
        var sens = sensitivity * (isZoomed ? zoomSensitivityMultiplier : 1);
        mouse.x += Input.GetAxis("Mouse X") * sens;
        mouse.y -= Input.GetAxis("Mouse Y") * sens;
        mouse.y = Mathf.Clamp(mouse.y, -90f, 90f);
        player.localRotation = Quaternion.Euler(0, mouse.x, 0);
        transform.localRotation = Quaternion.Euler(mouse.y, mouse.x, 0);
    }

    public Vector3 GetCrosshairTarget()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, crosshairLayerMask))
        {
            return hit.point;
        }
        else
        {
            return transform.position + transform.TransformDirection(Vector3.forward * 100f);
        }
    }
    
    public void Zoom(float targetFOV)
    {
        isZoomed = true;
        this.targetFOV = targetFOV;
        viewmodelCamera.SetActive(false);
    }

    public void ResetZoom()
    {
        isZoomed = false;
        targetFOV = initialFOV;
        viewmodelCamera.SetActive(true);
    }

    public void Shake(float intensity)
    {
        mainCamera.transform.DOKill();
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.DOShakePosition(Mathf.Lerp(0.2f, 0.8f, intensity), Mathf.Lerp(0.2f, 1.2f, intensity));
    }
}
