using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Gun : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private int bulletCount;
    [SerializeField] private float randomSpread;

    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRechargeRate;
    [SerializeField] private float energyCost;

    [SerializeField] private Transform nozzle;
    [SerializeField] private GameObject bulletPrefab;
    public PlayerController player;

    private float energy;
    private bool ready;
    private float timer;

    // Start is called before the first frame update
    void Awake()
    {
        energy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (energy < maxEnergy)
        {
            energy += energyRechargeRate * Time.deltaTime;
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ready = true;
            }
        }
    }

    public void Fire()
    {
        if (ready && energy >= energyCost)
        {
            ready = false;
            timer = fireRate;
            energy -= energyCost;

            Bullet bullet = LeanPool.Spawn(bulletPrefab).GetComponent<Bullet>();
        }
    }
}