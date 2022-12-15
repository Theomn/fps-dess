using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

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
    [SerializeField] private float switchSpeed;

    [Header("Consummables")]
    [SerializeField] private List<ConsummablePair> consummables;

    [Header("Weapon Sway")]
    [SerializeField] private float swaySensitivity;
    [SerializeField] private float swayMaxAmount;
    [SerializeField] private float swaySmooth;

    public enum State
    {
        Ready,
        SwitchingOut,
        SwitchingIn,
        Dying
    }

    private State state;
    private PlayerGun equippedGun;
    private int equippedGunId;
    private CameraController cam;
    private HUDController hud;
    private float switchOutTimer;
    private float switchInTimer;
    private int pendingSwitchId;

    void Start()
    {
        cam = CameraController.instance;
        hud = HUDController.instance;
        foreach(PlayerGun gun in guns)
        {
            gun.Holster();
        }
        EquipGun(0);
    }
    

    void Update()
    {
        if (state == State.Ready)
        {
            // Fire
            if (Input.GetButton("Fire1"))
            {
                FireGun();
            }

            // Grab
            if (Input.GetKeyDown(KeyCode.E))
            {
                Grab();
            }

            // Zoom
            if (Input.GetButton("Fire2") && equippedGun.canZoom)
            {
                cam.Zoom(25);
            }
            if (Input.GetButtonUp("Fire2"))
            {
                cam.ResetZoom();
            }

            // Weapon switch
            if (!equippedGun.cannotSwitch)
            {
                // Alpha 1 to 5
                for (int i = 0; i < guns.Count; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    {
                        SwitchTo(i);
                    }
                }

                // Scroll Wheel
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    SwitchTo(equippedGunId + 1);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
                {
                    SwitchTo(equippedGunId - 1);
                }
            }
        }

        if (state == State.SwitchingOut)
        {
            switchOutTimer -= Time.deltaTime;
            if (switchOutTimer <= 0)
            {
                switchInTimer = switchSpeed / 2f;
                state = State.SwitchingIn;
                transform.DOLocalMove(Vector3.zero, switchSpeed / 2);
                EquipGun(pendingSwitchId);
            }
        }

        if (state == State.SwitchingIn)
        {
            switchInTimer -= Time.deltaTime;
            if (switchInTimer <= 0)
            {
                state = State.Ready;
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
                SwitchTo(0);
            }
        }
    }

    // Activates weapon switch animation
    private void SwitchTo(int id)
    {
        if (equippedGunId == id)
        {
            return;
        }
        pendingSwitchId = id;
        switchOutTimer = switchSpeed / 2f;
        state = State.SwitchingOut;
        transform.DOLocalMove(new Vector3(-0.1f, -0.3f, -1f), switchSpeed / 2);
    }

    // Changes the equipped gun without switch animation
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
        if (!equippedGun.cannotSwitch)
            SwitchTo(guns.Count - 1);
        return true;
    }

    public void FlagForDeath()
    {
        state = State.Dying;
        transform.DOLocalMove(new Vector3(0, -0.5f, -0.5f), 0.5f);

    }
}
