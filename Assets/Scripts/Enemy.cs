using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enemy Health
    [Header("Health")]
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyMaxHealth;

    [Header("Aim")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject EnemyBulletPrefab; 
    [SerializeField] private float enemyRotationSpeed;
    [SerializeField] private float detectionRange;
    

    private float distanceToPlayer;
    private Transform player;



    private void Start()
    {
        enemyMaxHealth = 1000;

        // Retrieve the only instance of PlayerController in the scene automatically
        player = PlayerController.Instance.transform;
    }

    void Update()
    {
        //calcul de la distance entre enemi et le joueur
        distanceToPlayer = Vector3.Distance(transform.position, player.position);


        //Gestion de health
        if (enemyHealth >= enemyMaxHealth)
        {
            enemyHealth = enemyMaxHealth;   
        }
        if(enemyHealth <= 0)
        {
            enemyHealth = 0;
            Die();
        }

        EnemyDetection();

    }

    void EnemyDetection()
    {
        if (distanceToPlayer <= detectionRange)
        {
            var step = enemyRotationSpeed * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
