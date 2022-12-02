using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using DG.Tweening;

public class Gun : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Time in second between shots.")]
    [SerializeField] private float fireRate;
    [Tooltip("Number of bullets fired per shot.")]
    [SerializeField] private int bulletCount;
    [Tooltip("How much each bullet deviates from the nozzle direction.")]
    [SerializeField] private float randomSpread;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private BulletData bulletData;

    [Header("Energy")]
    [SerializeField] private float maxEnergy;
    [Tooltip("How much energy regenerates per second.")]
    [SerializeField] private float energyRechargeRate;
    [Tooltip("Cost of one trigger pull.")]
    [SerializeField] private float energyCost;

    [Header("Barrel")]
    [Tooltip("If true, the nozzle will rotate where the crosshair collides with the world everytime the gun is fired. " +
        "Set to false for guns equipped on enemies.")]
    [SerializeField] private bool fireAtCrosshair;
    [Tooltip("If true, the nozzle will rotate towards the player every time the gun is fired. " +
        "Set to false for guns equipped on player.")]
    [SerializeField] private bool fireAtPlayer;
    [SerializeField] private bool canZoom;
    [Tooltip("Position and direction where bullets spawn.")]
    [SerializeField] private List<Transform> nozzles;

    [Space(10f)]
    [Header("Juice")]
    [Header("Recoil")]
    [SerializeField] private float rotationRecoil;
    [SerializeField] private float positionRecoil;
    [SerializeField] private float recoilDuration;

    [Header("Nozzle Flash")]
    [Tooltip("Set to false to set flash transforms separately from nozzles")]
    [SerializeField] private bool nozzlesAreFlashes;
    [Tooltip("Spheres that flashs at the tip of the nozzle when gun is fired.")]
    [SerializeField] private List<Transform> nozzleFlashes;
    [SerializeField] private float nozzleFlashSize;
    [SerializeField] private float nozzleFlashDuration;

    [Header("Sound")]
    [Tooltip("id of the sound to play when the gun is fired.")]
    [SerializeField] private string fireSound;

    private float energy;
    private bool ready;
    private float fireRateTimer;
    private int nozzleId;
    private Transform currentNozzle;
    private Transform currentNozzleFlash;
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    

    void Awake()
    {
        energy = maxEnergy;
        ready = true;
        if (nozzleFlashes.Count > 0 && (nozzles.Count != nozzleFlashes.Count))
        {
            Debug.LogWarning("Gun does not contain as many nozzle flashes as there are nozzles", this);
        }
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
    }
    

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

            for (int i = 0; i < bulletCount; i++)
            {
                NextNozzle();
                bullet = LeanPool.Spawn(bulletPrefab).GetComponent<Bullet>();
                bullet.transform.localPosition = currentNozzle.position;
                if (fireAtCrosshair)
                {
                    currentNozzle.LookAt(CameraController.Instance.GetCrosshairTarget());
                }
                else if (fireAtPlayer)
                {
                    currentNozzle.LookAt(PlayerController.Instance.transform.position);
                }
                bullet.transform.rotation = currentNozzle.rotation;
                bullet.transform.eulerAngles += new Vector3(Random.Range(-randomSpread, randomSpread), Random.Range(-randomSpread, randomSpread), 0);
                bullet.Spawn(bulletData);
            }

            if (currentNozzleFlash)
            {
                currentNozzleFlash.localScale = Vector3.zero;
                currentNozzleFlash.DOPunchScale(Vector3.one * nozzleFlashSize, nozzleFlashDuration, 0, 0).SetEase(Ease.OutCubic);
            }
            AudioManager.Instance.PlaySoundAtPosition(fireSound, transform.position);
            if (recoilDuration > 0)
            {
                transform.DORewind();
                transform.localRotation = initialRotation;
                transform.DOPunchRotation(Vector3.left * rotationRecoil, recoilDuration, 0, 0);
                transform.localPosition = initialPosition;
                transform.DOPunchPosition(Vector3.back * positionRecoil, recoilDuration, 0, 0);
            }
        }
    }

    private void NextNozzle()
    {
        nozzleId++;
        if (nozzleId >= nozzles.Count)
        {
            nozzleId = 0;
        }
        currentNozzle = nozzles[nozzleId];
        if (nozzlesAreFlashes)
        {
            currentNozzleFlash = nozzles[nozzleId];
        }
        else if (nozzleFlashes.Count > 0)
        {
            currentNozzleFlash = nozzleFlashes[nozzleId];
        }
    }

    public bool CanZoom()
    {
        return canZoom;
    }
}