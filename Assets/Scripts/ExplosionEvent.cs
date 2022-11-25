using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class ExplosionEvent : Event
{
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;

    public override void Spawn()
    {
        lifetimeTimer = lifetime;
        foreach(Collider entity in Physics.OverlapSphere(transform.position, explosionRadius))
        {
            if (entity.gameObject.layer == Layer.playerLayer)
            {
                Debug.Log("player collided");
                var player = entity.GetComponent<PlayerController>();
                if (!player)
                {
                    return;
                }
                player.ExplosionForce(explosionForce, transform.position);
            }
        }
    }

    public override void Despawn()
    {
        LeanPool.Despawn(this);
    }

 
}
