using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool damagesPlayer;
    [SerializeField] private bool damagesEnemies;
    [SerializeField] public float damage;
    [SerializeField] private float speed;
    [SerializeField] private int maxPierceCount;
    [SerializeField] private float lifetime;
    [SerializeField] private float gravity;

    // This will be spawned when the bullet despawns. Used for explosions, gas clouds, etc.
    [SerializeField] private GameObject endEvent;

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
    public void Initialize()
    {
        pierceCount = 0;
        lifetimeTimer = lifetime;
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
        rb.MovePosition(Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * speed * Time.fixedDeltaTime, float.MaxValue));

        // Apply gravity
        if (gravity != 0)
        {
            rb.AddForce(Vector3.down * gravity * 10 * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void Despawn()
    {
        if (endEvent)
        {
            LeanPool.Spawn(endEvent);
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
            if (damagesEnemies)
            {
                enemy.Damage(this);
                pierceCount++;
                if (pierceCount >= maxPierceCount)
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
            if (damagesPlayer)
            {
                // TODO
                pierceCount++;
                if (pierceCount >= maxPierceCount)
                {
                    Despawn();
                }
            }
        }

        else if (layer == Layer.groundLayer)
        {
            Despawn();
        }
    }
}