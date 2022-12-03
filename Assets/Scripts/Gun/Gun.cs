using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using DG.Tweening;

public abstract class Gun : MonoBehaviour
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

    [Header("Nozzle Flash")]
    [SerializeField] private float nozzleFlashSize;
    [SerializeField] private float nozzleFlashDuration;

    [Header("Sound")]
    [Tooltip("id of the sound to play when the gun is fired.")]
    [SerializeField] private string fireSound;

    protected bool ready;
    private float fireRateTimer;
    protected Transform activeNozzle;
    protected Transform activeNozzleFlash;
    
    protected virtual void Awake()
    {
        ready = true;
    }

    protected virtual void Update()
    {
        if (fireRateTimer > 0)
        {
            fireRateTimer -= Time.deltaTime;
            if (fireRateTimer <= 0)
            {
                ready = true;
            }
        }
    }

    public virtual bool Fire()
    {
        if (!ready)
        {
            return false;
        }
        
        Bullet bullet;
        for (int i = 0; i < bulletCount; i++)
        {
            ArmNozzle();
            bullet = LeanPool.Spawn(bulletPrefab).GetComponent<Bullet>();
            bullet.transform.localPosition = activeNozzle.position;
            bullet.transform.rotation = activeNozzle.rotation;
            bullet.transform.eulerAngles += new Vector3(Random.Range(-randomSpread, randomSpread), Random.Range(-randomSpread, randomSpread), 0);
            bullet.Spawn(bulletData);

            if (activeNozzleFlash && nozzleFlashDuration > 0)
            {
                activeNozzleFlash.DOKill();
                activeNozzleFlash.localScale = Vector3.zero;
                activeNozzleFlash.DOPunchScale(Vector3.one * nozzleFlashSize, nozzleFlashDuration, 0, 0).SetEase(Ease.OutCubic);
            }
        }

        ready = false;
        fireRateTimer = fireRate;
        
        AudioManager.Instance.PlaySoundAtPosition(fireSound, transform.position);
        return true;
    }

    protected virtual void ArmNozzle()
    {
        
    }
}