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
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject EnemyBulletPrefab; 
    [SerializeField] private int enemyRotationSpeed;



    void update()
    {
        //Gestion de health
        if(enemyHealth >= enemyMaxHealth)
        {
            enemyHealth = enemyMaxHealth;   
        }
        if(enemyHealth <= 0)
        {
            die();
        }


    }


    void die()
    {
        Destroy(gameObject);
    }
}
