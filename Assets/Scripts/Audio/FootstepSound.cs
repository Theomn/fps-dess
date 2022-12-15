using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : SingletonMonoBehaviour<FootstepSound>
{
    [SerializeField] private float footstepInterval;
    [SerializeField] private float speedThreshold;
    [SerializeField] private string footstepSoundId;
    [SerializeField] private string hitSoundId;

    private Rigidbody rb;
    private PlayerController player;
    private float footstepTimer;
    private bool isMoving;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if(rb.velocity.magnitude > speedThreshold && player.GetState() == PlayerController.State.Grounded)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                AudioManager.instance.PlaySound(footstepSoundId, audioSource);
                footstepTimer = footstepInterval;
            }
        }
    }

    public void PlayHitSound()
    {
        AudioManager.instance.PlaySound(hitSoundId, audioSource);
    }
}
