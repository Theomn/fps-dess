using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Gun : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Time in second between shots.")]
    [SerializeField] private float fireRate;
    [Tooltip("Number of bullets fired per shot.")]
    [SerializeField] private int bulletCount;
    [Tooltip("How much each bullet deviates from the nozzle direction.")]
    [SerializeField] private float randomSpread;
    [SerializeField] private BulletData bulletData;

    [Header("Energy")]
    [SerializeField] private float maxEnergy;
    [Tooltip("How much energy regenerates per second.")]
    [SerializeField] private float energyRechargeRate;
    [Tooltip("Cost of one trigger pull.")]
    [SerializeField] private float energyCost;

    [Header("Barrel")]
    [Tooltip("Position and direction where bullets spawn.")]
    [SerializeField] private Transform nozzle;
    [Tooltip("If true, the nozzle will rotate where the crosshair collides with the world everytime the gun is fired. " +
        "Set to false for guns equipped on enemies.")]
    [SerializeField] private bool fireAtCrosshair;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("id of the sound to play when the gun is fired.")]
    [SerializeField] private string fireSound;

    private AudioManager audioManager;
    private AudioSource audioSource;

    private float energy;
    private bool ready;
    private float fireRateTimer;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
                audioSource.PlayOneShot(audioManager.GetClip(fireSound));
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
                bullet.Spawn(bulletData);
            }
        }
    }
}