using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBelt : SingletonMonoBehaviour<ItemBelt>
{
    [SerializeField] private float grabRange;
    [SerializeField] private List<PlayerGun> guns;

    [Header("Consummables")]
    [SerializeField] private PlayerGun medkit;
    [SerializeField] private PlayerGun shield;
    [SerializeField] private PlayerGun barrel;
    [SerializeField] private bool hasMedkit;
    [SerializeField] private bool hasShield;
    [SerializeField] private bool hasBarrel;
    [SerializeField] private bool isHoldingGrabbable;

    [Header("Weapon Sway")]
    [SerializeField] private float swaySensitivity;
    [SerializeField] private float swayMaxAmount;
    [SerializeField] private float swaySmooth;

    private PlayerGun equippedGun;
    private int equippedGunId;
    private CameraController cam;
    private HUDController hud;

    void Start()
    {
        cam = CameraController.Instance;
        hud = HUDController.Instance;
        foreach(PlayerGun gun in guns)
        {
            gun.Holster();
        }
        EquipGun(0);
    }
    

    void Update()
    {
        // Fire
        if (Input.GetButton("Fire1"))
        {
            FireGun();
        }

        // Grab barrel
        if (Input.GetKeyDown(KeyCode.E))
        {

        }

        // Zoom
        if (Input.GetButtonDown("Fire2") && equippedGun.canZoom)
        {
            cam.Zoom(25);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            cam.ResetZoom();
        }

        // Change weapon
        if (!isHoldingGrabbable) // Cannot change weapon if holding item
        {
            for (int i = 0; i < guns.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    EquipGun(i);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                EquipGun(equippedGunId + 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                EquipGun(equippedGunId - 1);
            }
        }

        SwayGun();
        hud.SetEnergy(equippedGun.GetEnergy());
    }

    private void FireGun()
    {
        if (equippedGun.Fire())
        {
            cam.ResetZoom();
            if (equippedGun.isConsummable)
            {
                guns.RemoveAt(equippedGunId);
                EquipGun(equippedGunId);
            }
            isHoldingGrabbable = false;
        }
    }

    private void EquipGun(int id)
    {
        if(id >= guns.Count)
        {
            id = 0;
        }
        else if (id < 0)
        {
            id = guns.Count - 1;
        }
        equippedGun?.Holster();
        equippedGun = guns[id];
        equippedGunId = id;
        equippedGun.Unholster();
        cam.ResetZoom();
    }

    private void Grab()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabRange, Layer.enemy))
        {
            var grabbable = hit.collider.gameObject.GetComponent<Grabbable>();
            if (grabbable)
            {
                grabbable.Grab();
                isHoldingGrabbable = true;
            }
        }
    }

    private void SwayGun()
    {
        float dx = Input.GetAxisRaw("Mouse X") * swaySensitivity;
        float dy = Input.GetAxisRaw("Mouse Y") * swaySensitivity;
        dx = Mathf.Clamp(dx, -swayMaxAmount, swayMaxAmount);
        dy = Mathf.Clamp(dy, -swayMaxAmount, swayMaxAmount);
        var target = Quaternion.Euler(-dy, dx, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, swaySmooth * Time.deltaTime);
    }

    public bool AddGun(PlayerGun gun)
    {
        if (guns.Exists(g => g.id == gun.id))
            return false;

        guns.Add(gun);
        EquipGun(guns.Count - 1);
        return true;
    }
}
