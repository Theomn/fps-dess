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
    [SerializeField] private int pierceCount;
    [SerializeField] private float lifetime;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject endEvent;

    private Rigidbody rb;
    private float lifetimeTimer;
    private TrailRenderer trail;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    public void Initialize()
    {
        lifetimeTimer = lifetime;
        trail?.Clear();
    }


    // Update is called once per frame
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
        rb.MovePosition(Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * speed * Time.fixedDeltaTime, float.MaxValue));
        rb.AddForce(Vector3.down * gravity * 10 * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void Despawn()
    {
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
            }
        }
    }
}