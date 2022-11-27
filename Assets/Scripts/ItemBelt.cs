using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBelt : MonoBehaviour
{
    [SerializeField] private List<Gun> guns;
    private Gun equippedGun;
    private int equippedGunId;

    void Start()
    {
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
    }
}
