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

    [Header("Tracking Behaviour")]
    [Tooltip("If activated, the enemy will turn towards the player when within range.")]
    [SerializeField] private bool tracksPlayer;
    [SerializeField] private float trackingSpeed;
    [SerializeField] private float trackingRange;
    
    [Header("Fire Behaviour")]
    [Tooltip("If activated, the enemy is able to shoot the equipped gun.")]
    [SerializeField] private bool firesBullets;
    [SerializeField] private float fireRange;
    [SerializeField] private Gun gun;

    [Header("References")]
    [SerializeField] List<Collider> colliders;

    private float health;
    protected float distanceToPlayer;
    protected Transform player;
    private float fireRateTimer;
    [Tooltip("False if tracking behaviour is enabled and the player is further than 15 degress from the enemy.")]
    protected bool facingPlayer;


    protected void Start()
    {
        // Retrieve the only instance of PlayerController in the scene automatically
        player = PlayerController.Instance.transform;

        health = maxHealth;
        facingPlayer = true;
    }

    protected void Update()
    {
        //calcul de la distance entre enemi et le joueur
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (tracksPlayer)
        {
            TrackingBehaviour();
        }

        if (firesBullets)
        {
            FireBehaviour();
        }

    }

    /// <summary>
    /// Turns toward the player when within tracking range
    /// </summary>
    protected void TrackingBehaviour()
    {
        if (distanceToPlayer <= trackingRange)
        {
            var step = trackingSpeed * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            facingPlayer = Quaternion.Angle(transform.rotation, targetRotation) < 15f;
        }
    }

   

    /// <summary>
    /// Fires in front of itself when within fire range
    /// </summary>
    protected void FireBehaviour()
    {
        if (distanceToPlayer <= fireRange && facingPlayer)
        {
            gun.Fire();
        }
    }

    // Called by a bullet when it collides with that enemy
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy();
        }
    }

    public virtual void ExplosionForce(float explosionForce, Vector3 explosionPosition)
    {

    }

    protected void Destroy()
    {
        foreach(Collider coll in colliders)
        {
            coll.enabled = false;
        }
        if (endEvent)
        {
            var evt = LeanPool.Spawn(endEvent);
            evt.transform.position = transform.position;
            evt.Spawn();
        }
        Destroy(gameObject);
    }
}
