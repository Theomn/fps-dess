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
    [SerializeField] private bool fireAtCrosshair;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private string fireSound;

    private AudioManager audioManager;
    

    private float energy;
    private bool ready;
    private float fireRateTimer;

    // Start is called before the first frame update
    void Awake()
    {
       
        energy = maxEnergy;
        ready = true;
    }

    void Start()
    {
        audioManager = AudioManager.Instance;
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

        if (fireRateTimer > 0)
        {
            fireRateTimer -= Time.deltaTime;
            if (fireRateTimer <= 0)
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
            fireRateTimer = fireRate;
            energy -= energyCost;
            Bullet bullet;
            if (!string.IsNullOrEmpty(fireSound))
            {
                audioManager.Play(fireSound);
            }

            

            for (int i = 0; i < bulletCount; i++)
            {
                bullet = LeanPool.Spawn(bulletPrefab).GetComponent<Bullet>();
                bullet.transform.localPosition = nozzle.position;
                if (fireAtCrosshair)
                {
                    nozzle.LookAt(CameraController.Instance.GetCrosshairTarget());
                }
                bullet.transform.rotation = nozzle.rotation;
                bullet.transform.eulerAngles += new Vector3(Random.Range(-randomSpread, randomSpread), Random.Range(-randomSpread, randomSpread), 0);
                bullet.Initialize();
            }
        }
    }
}