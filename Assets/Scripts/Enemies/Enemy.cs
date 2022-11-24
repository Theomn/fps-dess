using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    //Enemy Health
    [Header("Health")]
    [SerializeField] private int maxHealth;

    [Header("Tracking Behaviour")]
    [SerializeField] private bool tracksPlayer;
    [SerializeField] private float trackingSpeed;
    [SerializeField] private float trackingRange;

    [Header("Fire Behaviour")]
    [SerializeField] private bool firesBullets;
    [SerializeField] private float fireRange;
    [SerializeField] private Gun gun;


    private float health;
    private float distanceToPlayer;
    private Transform player;
    private float fireRateTimer;


    private void Start()
    {
        // Retrieve the only instance of PlayerController in the scene automatically
        player = PlayerController.Instance.transform;

        health = maxHealth;
    }

    void Update()
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
    void TrackingBehaviour()
    {
        if (distanceToPlayer <= trackingRange)
        {
            var step = trackingSpeed * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        }
    }

    /// <summary>
    /// Fires in front of itself when within fire range
    /// </summary>
    void FireBehaviour()
    {
        if (distanceToPlayer <= fireRange)
        {
            gun.Fire();
        }
    }

    // Called by a bullet when it collides with that enemy
    public void Damage(Bullet bullet)
    {
        health -= bullet.damage;
        if (health <= 0)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
