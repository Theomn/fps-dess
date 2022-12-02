using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBelt : SingletonMonoBehaviour<ItemBelt>
{
    [SerializeField] private List<Gun> guns;

    [Header("Weapon Sway")]
    [SerializeField] private float swaySensitivity;
    [SerializeField] private float swayMaxAmount;
    [SerializeField] private float swaySmooth;

    private Gun equippedGun;
    private int equippedGunId;
    private CameraController cam;

    void Start()
    {
        cam = CameraController.Instance;
        foreach(Gun gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
        EquipGun(0);
    }
    

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            equippedGun.Fire();
            cam.ResetZoom();
        }

        if (Input.GetButtonDown("Fire2") && equippedGun.CanZoom())
        {
            cam.Zoom(25);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            cam.ResetZoom();
        }

        for (int i = 0; i < guns.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 +i ))
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

        SwayGun();
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
        equippedGun?.gameObject.SetActive(false);
        equippedGun = guns[id];
        equippedGunId = id;
        equippedGun.gameObject.SetActive(true);
        if (!equippedGun.CanZoom())
        {
            cam.ResetZoom();
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
}
