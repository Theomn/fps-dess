using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pickup : MonoBehaviour
{
    [SerializeField] private Consummable consummable;

    private void Update()
    {
        transform.Rotate(Vector3.up, 70 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.player)
        {
            bool didNotHaveGun = ItemBelt.Instance.AddConsummable(consummable);
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
