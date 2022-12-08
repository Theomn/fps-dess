using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] public struct ConsummablePair
{
    public Consummable key;
    public PlayerGun gun;
}

public enum Consummable
{
    Medkit,
    Shield,
    Barrel
}

public class ItemBelt : SingletonMonoBehaviour<ItemBelt>
{
    [SerializeField] private float grabRange;
    [SerializeField] private List<PlayerGun> guns;

    [Header("Consummables")]
    [SerializeField] private List<ConsummablePair> consummables;

    [Header("Weapon Sway")]
    [SerializeField] private float swaySensitivity;
    [SerializeField] private float swayMaxAmount;
    [SerializeField] private float swaySmooth;

    private PlayerGun equippedGun;
    private int equippedGunId;
    private CameraController cam;
    private HUDController hud;
    private bool isHoldingGrabbable;
    private bool lockFire;

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
        if (!lockFire && Input.GetButton("Fire1"))
        {
            FireGun();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            lockFire = false;
        }

        // Grab barrel
        if (!isHoldingGrabbable) // Cannot grab if holding item
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Grab();
            }
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
                lockFire = true;
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
        Debug.DrawLine(transform.position, transform.position + transform.forward * grabRange, Color.red, 3f);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabRange, LayerMask.GetMask("Enemy")))
        {
            var grabbable = hit.collider.gameObject.GetComponentInParent<Grabbable>();
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

    public bool AddConsummable(Consummable consummableType)
    {
        var consummable = consummables.Find(c => c.key == consummableType).gun;

        // Do not equip consummable if player already has it
        if (guns.Exists(g => g.id == consummable.id))
            return false;

        guns.Add(consummable);
        if (!isHoldingGrabbable)
            EquipGun(guns.Count - 1);
        return true;
    }
}
