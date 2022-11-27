using System.Collections;
using System;
using UnityEngine;
using Lean.Pool;

[Serializable]
public struct BulletData
{
    [Tooltip("If false, ignores objects on the Player layer.")]
    public bool damagesPlayer;
    [Tooltip("If false, ignores object on the Enemy layer.")]
    public bool damagesEnemies;
    [Tooltip("Damage dealt per bullet.")]
    public float damage;
    [Tooltip("Speed at which the bullet travels.")]
    public float speed;
    [Tooltip("How many entities the bullet will go through before despawning. Does not pierce objects on Ground layer.")]
    public int maxPierceCount;
    [Tooltip("Time in second before bullet despawns without colliding with anything.")]
    public float lifetime;
    [Tooltip("Adds an arc to trajectory.")]
    public float gravity;
    [Tooltip("If true, the bullet will despawn as soon as it touches a ground element.")]
    public bool destroyOnGroundContact;
    [Tooltip("Object that will be spawned when and where the bullet despawns. (Explosions, gas clouds...)")]
    public Event endEvent;
}

public class Bullet : MonoBehaviour
{
    private BulletData data;

    private Rigidbody rb;
    private TrailRenderer trail;

    private float lifetimeTimer;
    private int pierceCount;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    // Called when the gun spawns the bullet
    public void Spawn(BulletData data)
    {
        this.data = data;
        pierceCount = 0;
        lifetimeTimer = data.lifetime;
        trail?.Clear();
    }
    
    void Update()
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

    private void FixedUpdate()
    {
        // Travel forward
        rb.MovePosition(Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * data.speed * Time.fixedDeltaTime, float.MaxValue));

        // Apply gravity
        if (data.gravity != 0)
        {
            rb.AddForce(Vector3.down * data.gravity * 10 * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void Despawn()
    {
        if (data.endEvent)
        {
            var evt = LeanPool.Spawn(data.endEvent);
            evt.transform.position = transform.position;
            evt.Spawn();
        }
        LeanPool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        var layer = other.gameObject.layer;
        if(layer == Layer.enemyLayer)
        {
            var enemy = other.GetComponent<Enemy>();
            if (!enemy)
            {
                Debug.LogWarning("Bullet collided with an object on the Enemy layer but with no Enemy component");
                return;
            }
            if (data.damagesEnemies)
            {
                enemy.Damage(data.damage);
                pierceCount++;
                if (pierceCount >= data.maxPierceCount)
                {
                    Despawn();
                }
            }
        }

        else if (layer == Layer.playerLayer)
        {
            var player = other.GetComponent<PlayerController>();
            if (!player)
            {
                Debug.LogWarning("Bullet collided with an object on the Player layer but with no PlayerController component");
                return;
            }
            if (data.damagesPlayer)
            {
                // TODO
                pierceCount++;
                if (pierceCount >= data.maxPierceCount)
                {
                    Despawn();
                }
            }
        }

        else if (layer == Layer.groundLayer)
        {
            if (data.destroyOnGroundContact)
            {
                Despawn();
            }
        }
    }
}