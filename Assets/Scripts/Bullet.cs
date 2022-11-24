using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool damagesPlayer;
    [SerializeField] private bool damagesEnemies;
    [SerializeField] private float speed;
    [SerializeField] public float damage { get; private set; }
    [SerializeField] private int pierceCount;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject endEvent;

    private Rigidbody rb;
    private float lifetimeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        lifetimeTimer = lifetime;
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
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * speed * Time.deltaTime, float.MaxValue);
    }

    private void Despawn()
    {
        LeanPool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        // future usine a gaz
    }
}