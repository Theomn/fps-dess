using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enemy Health
    [Header("Health")]
    [SerializeField] private int EnemyHealth;
    [SerializeField] private int EnemyMaxHealth;

    [Header("Movement")]
    [SerializeField] private int EnemySpeed;
    [SerializeField] private int EnemyRotationSpeed;
}
