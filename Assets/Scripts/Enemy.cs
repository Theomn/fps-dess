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
    [SerializeField] private Transform target;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject EnemyBulletPrefab; 
    [SerializeField] private int enemyRotationSpeed;

    private float distanceToPlayer; //calcul de la distance entre le turret et le joueur.



    void Update()
    {
        //calcul de la distance entre enemi et le joueur
        distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, target.position);
        Debug.Log(distanceToPlayer);


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
       
        TargetPlayer();
    }

    void TargetPlayer()
    {
        /*

        if(target == null) //ajouter || player.health <= 0
        {
            target = 
        }
        */
                
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
