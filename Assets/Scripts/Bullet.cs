using System;
using UnityEngine;
using Lean.Pool;

[Serializable]
public struct BulletData
{
    [Header("Main Data")]
    [Tooltip("If false, ignores objects on the Player layer.")]
    public bool damagesPlayer;
    [Tooltip("If false, ignores object on the Enemy layer.")]
    public bool damagesEnemies;
    [Tooltip("Damage dealt per bullet.")]
    public float damage;
    [Tooltip("Time in second before bullet despawns.")]
    public float lifetime;
    [Tooltip("Object that will be spawned when and where the bullet despawns. (Explosions, gas clouds...)")]
    public Event endEvent;
    [Header("Projectile Data")]
    [Tooltip("Speed at which the bullet travels.")]
    public float speed;
    [Tooltip("Adds an arc to trajectory.")]
    public float gravity;
    [Tooltip("How many entities the bullet will go through before despawning. Does not pierce objects on Ground layer.")]
    public int maxPierceCount;
    [Tooltip("If true, the bullet will despawn as soon as it touches a ground element.")]
    public bool destroyOnGroundContact;
    [Header("Raycast Data")]
    public float maxRange;
}

public abstract class Bullet : MonoBehaviour
{
    protected BulletData data;
    protected float lifetimeTimer;


    protected virtual void Update()
    {
        if (lifetimeTimer > 0f)
        {
            lifetimeTimer -= Time.deltaTime;
            if (lifetimeTimer <= 0)
            {
                Despawn();
            }
        }
    }

    // Called when the gun spawns the bullet
    public virtual void Spawn(BulletData data)
    {
        this.data = data;
        lifetimeTimer = data.lifetime;
    }

    protected virtual void Despawn()
    {
        LeanPool.Despawn(this);
    }

    protected int Deliver(Collider other)
    {
        var layer = other.gameObject.layer;
        if (layer == Layer.enemy)
        {
            var enemy = other.GetComponent<Enemy>();
            if (!enemy)
            {
                Debug.LogWarning("Bullet collided with an object on the Enemy layer but with no Enemy component");
                return 0;
            }
            if (data.damagesEnemies)
            {
                enemy.Damage(data.damage);
                return layer;
            }
        }

        else if (layer == Layer.player)
        {
            var player = other.GetComponent<PlayerController>();
            if (!player)
            {
                Debug.LogWarning("Bullet collided with an object on the Player layer but with no PlayerController component");
                return 0;
            }
            if (data.damagesPlayer)
            {
                player.Damage(data.damage);
                return layer;
            }
        }

        else if (layer == Layer.ground)
        {
            return layer;
        }

        return 0;
    }
}
