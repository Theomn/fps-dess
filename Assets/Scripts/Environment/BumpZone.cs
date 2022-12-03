using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpZone : MonoBehaviour
{
    [SerializeField] private Vector3 bumpForce;
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.player)
        {
            var player = other.GetComponent<PlayerController>();
            if (!player)
            {
                return;
            }
            player.Damage(damage);
            player.AddForce(bumpForce);
        }
    }
}
