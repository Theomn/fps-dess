using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Lean.Pool;


public class Enemy : MonoBehaviour, IResettable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private Event endEvent;

    [Header("Misc")]
    [Tooltip("How long it takes to destroy the enemy once it's killed.")]
    [SerializeField] private float destroyTime;
    [SerializeField] private GameObject deathFX;
    [SerializeField, HideInInspector] private string hitSound;
    

    private float health;
    protected float distanceToPlayer;
    protected Transform player;
    private Collider[] colliders;
    private EnemyBehaviour[] behaviours;
    private MeshRenderer[] regularMeshes;
    private MeshRenderer[] hitFlashMeshes;
    protected bool flaggedForDestroy;
    private bool soundPlayed;
    private AudioSource audioSource;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float destroyTimer;
    private float hitFlashTimer;
    private List<Door> doors;


    protected virtual void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        behaviours = GetComponentsInChildren<EnemyBehaviour>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    protected virtual void Start()
    {
        // Retrieve the only instance of PlayerController in the scene automatically
        player = PlayerController.instance.transform;
        CreateHitFlashMesh();
        Reset();
    }

    public virtual void Reset()
    {
        health = maxHealth;
        flaggedForDestroy = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        foreach (Collider coll in colliders)
        {
            coll.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        foreach(EnemyBehaviour behaviour in behaviours)
        {
            behaviour.enabled = true;
        }
        if (deathFX)
        {
            deathFX.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    protected virtual void Update()
    {
        if (flaggedForDestroy)
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0)
            {
                Destroy();
            }
            return;
        }

        if (hitFlashTimer > 0)
        {
            hitFlashTimer -= Time.deltaTime;
            if (hitFlashTimer <= 0)
            {
                hitFlashTimer = 0;
                HideHitFlash();
            }
        }

        soundPlayed = false;

        //calcul de la distance entre enemi et le joueur
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    protected virtual void FixedUpdate()
    {

    }

    public float GetDistanceToPlayer()
    {
        return distanceToPlayer;
    }

    // Called by a bullet when it collides with that enemy
    public void Damage(float damage)
    {
        health -= damage;
        //insert dammage sound here


        if (damage > 0)
        {
            ShowHitFlash();
            hitFlashTimer += Mathf.Lerp(0f, 0.3f, damage / 20);
            if (hitFlashTimer > 0.2f)
            {
                hitFlashTimer = 0.2f;
            }
            if (!soundPlayed)
            {
                soundPlayed = true;
                FootstepSound.instance.PlayHitSound();
            }

        }
        if (health <= 0)
        {
            FlagForDestroy();
        }
    }

    public virtual void ExplosionForce(float explosionForce, Vector3 explosionPosition)
    {

    }

    protected virtual void FlagForDestroy()
    {
        if (doors != null)
        {
            foreach (Door door in doors)
            {
                door.DecrementLock();
            }
        }
        foreach (Collider coll in colliders)
        {
            coll.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        foreach (EnemyBehaviour behaviour in behaviours)
        {
            behaviour.enabled = false;
        }
        flaggedForDestroy = true;
        destroyTimer = destroyTime;
        if (deathFX)
        {
            deathFX.SetActive(true);
        }
    }

    protected void Destroy()
    {
        if (endEvent)
        {
            var evt = LeanPool.Spawn(endEvent);
            evt.transform.position = transform.position;
            evt.Spawn();
        }
        if (deathFX)
        {
            deathFX.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void InstantKill()
    {
        health = 0;
        FlagForDestroy();
    }

    public void Register(Door door)
    {
        if (doors == null)
        {
            doors = new List<Door>();
        }
        doors.Add(door);
    }

    private void CreateHitFlashMesh()
    {
        regularMeshes = GetComponentsInChildren<MeshRenderer>();
        hitFlashMeshes = new MeshRenderer[regularMeshes.Length];
        for (int m = 0; m < regularMeshes.Length; m++)
        {
            var newMesh = Instantiate(regularMeshes[m], regularMeshes[m].transform, true);
            hitFlashMeshes[m] = newMesh;
            Destroy(newMesh.GetComponent<Collider>());
            var newMats = new Material[newMesh.materials.Length];
            for (int i = 0; i < newMats.Length; i++)
            {
                newMats[i] = DataAccessor.instance.swatches.GetMaterial("ENEMY_HITFLASH");
            }
            newMesh.materials = newMats;
            newMesh.enabled = false;
        }
    }

    private void ShowHitFlash()
    {
        for (int m = 0; m < regularMeshes.Length; m++)
        {
            regularMeshes[m].enabled = false;
            hitFlashMeshes[m].enabled = true;
        }
    }

    private void HideHitFlash()
    {
        for (int m = 0; m < regularMeshes.Length; m++)
        {
            regularMeshes[m].enabled = true;
            hitFlashMeshes[m].enabled = false;
        }
    }
}
