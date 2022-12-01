using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private float footstepInterval;
    [SerializeField] private float speedThreshold;
    [SerializeField] private string footstepSoundId;

    private Rigidbody rb;
    private PlayerController player;
    private float footstepTimer;
    private bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
    }
    
    void Update()
    {
        //if (!isMoving && rb.velocity.magnitude > speedThreshold)
        //{
        //    isMoving = true;
        //}
        //if (isMoving && rb.velocity.magnitude < speedThreshold)
        //{
        //    isMoving = false;
        //}

        if(rb.velocity.magnitude > speedThreshold && player.GetState() == PlayerController.State.Grounded)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                AudioManager.Instance.PlaySoundAtPosition(footstepSoundId, transform.position);
                footstepTimer = footstepInterval;
            }
        }
    }
}
