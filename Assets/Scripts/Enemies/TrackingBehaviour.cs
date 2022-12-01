using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBehaviour : EnemyBehaviour
{
    [SerializeField] private float trackingSpeed;
    [SerializeField] private float trackingRange;
    private bool facingPlayer;
    
    protected override void Update()
    {
        base.Update();
        if (distanceToPlayer <= trackingRange)
        {
            var step = trackingSpeed * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            facingPlayer = Quaternion.Angle(transform.rotation, targetRotation) < 15f;
        }
    }

    public bool IsFacingPlayer()
    {
        return facingPlayer;
    }
}
