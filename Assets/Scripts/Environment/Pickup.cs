using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PlayerGun grabbedGun;

    private void Start()
    {
        if (!grabbedGun)
        {
            Debug.LogError("Pickup has no Grabbed Gun associated. Pick one from the Guns on the main camera.", gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.player)
        {
            bool didNotHaveGun = ItemBelt.Instance.AddGun(grabbedGun);
            if (didNotHaveGun)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
