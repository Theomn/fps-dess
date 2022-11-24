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
        //health management
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        if (health <= 0)
        {
            health = 0;
            Destroy();
        }

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

    void TrackingBehaviour()
    {
        if (distanceToPlayer <= trackingRange)
        {
            var step = trackingSpeed * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        }
    }

    void FireBehaviour()
    {
        if (distanceToPlayer <= fireRange)
        {
            gun.Fire();
        }
    }

    void Damage(Bullet bullet)
    {
        health -= bullet.damage;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
