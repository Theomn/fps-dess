using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enemy Health
    [Header("Health")]
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyMaxHealth;

    [Header("Movement")]
    [SerializeField] private int enemySpeed;// besoin?
    [SerializeField] private int enemyRotationSpeed;//besoin?


    void update()
    {
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
