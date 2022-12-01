using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Lean.Pool;


public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private Event endEvent;

    [Header("Misc")]
    [Tooltip("How long it takes to destroy the enemy once it's killed.")]
    [SerializeField] private float destroyTime;
    [SerializeField] private GameObject deathFX;
    

    private float health;
    protected float distanceToPlayer;
    protected Transform player;
    private Collider[] colliders;
    private EnemyBehaviour[] behaviours;
    protected bool flaggedForDestroy;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float destroyTimer;


    protected virtual void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        behaviours = GetComponentsInChildren<EnemyBehaviour>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    protected virtual void Start()
    {
        // Retrieve the only instance of PlayerController in the scene automatically
        player = PlayerController.Instance.transform;

        Reset();

    }

    public virtual void Reset()
    {
        health = maxHealth;
        flaggedForDestroy = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        foreach (Collider coll in colliders)
        {
            coll.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        foreach(EnemyBehaviour behaviour in behaviours)
        {
            behaviour.enabled = true;
        }
        gameObject.SetActive(true);
    }

    protected virtual void Update()
    {
        if (flaggedForDestroy)
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0)
            {
                Destroy();
            }
            return;
        }

        //calcul de la distance entre enemi et le joueur
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    protected virtual void FixedUpdate()
    {

    }

    public float GetDistanceToPlayer()
    {
        return distanceToPlayer;
    }

    // Called by a bullet when it collides with that enemy
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            FlagForDestroy();
        }
    }

    public virtual void ExplosionForce(float explosionForce, Vector3 explosionPosition)
    {

    }

    protected virtual void FlagForDestroy()
    {
        foreach (Collider coll in colliders)
        {
            coll.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        foreach (EnemyBehaviour behaviour in behaviours)
        {
            behaviour.enabled = false;
        }
        flaggedForDestroy = true;
        destroyTimer = destroyTime;
        deathFX.SetActive(true);
    }

    protected void Destroy()
    {
        if (endEvent)
        {
            var evt = LeanPool.Spawn(endEvent);
            evt.transform.position = transform.position;
            evt.Spawn();
        }
        deathFX.SetActive(false);
        gameObject.SetActive(false);
    }
}
