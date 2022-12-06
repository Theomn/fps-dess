using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [SerializeField] private PlayerGun grabbedGun;

    public void Grab()
    {
        ItemBelt.Instance.AddGun(grabbedGun);
        gameObject.SetActive(false);
    }
}
