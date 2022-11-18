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
    [SerializeField] private Transform player;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject EnemyBulletPrefab; 
    [SerializeField] private float enemyRotationSpeed;
    

    private float distanceToPlayer; //calcul de la distance entre le turret et le joueur.

    private void Start()
    {
        enemyMaxHealth = 1000;
       
    }

    void Update()
    {
        //calcul de la distance entre enemi et le joueur
        distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, player.position);
        //Debug.Log(distanceToPlayer);


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
       
    }

    void FixedUpdate()
    {
        EnemyDetection();
    }

    void EnemyDetection()
    {
        if (distanceToPlayer <= 20)
        {
            //transform.LookAt(player.position); Ca marche mais on peut pas controller la vitesse.
            var step = enemyRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, player.rotation, step);
        }
                
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
