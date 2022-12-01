using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : EnemyBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float followRange;
    [Tooltip("If true, will only move when roughly facing player. Needs a TrackingBehaviour component on the same object.")]
    [SerializeField] private bool followWhenFacingPlayer;
    [Tooltip("Minimum distance from this enemy to player")]
    [SerializeField] private float hoverRange;

    private bool hasTarget;
    private Vector3 offset;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        bool canFollow = followWhenFacingPlayer ? isFacingPlayer : true;
        if (distanceToPlayer <= followRange && canFollow) 
        {
            if (!hasTarget)
            {
                AcquireTarget();
            }

            var targetPosition = (player.position + offset);
            var moveForce = Vector3.ClampMagnitude((targetPosition - transform.position), 1f) * acceleration;
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(moveForce, ForceMode.Acceleration);
            }
            if (Vector3.Distance(targetPosition, transform.position) < 0.5f)
            {
                AcquireTarget();
            }
        }
        else
        {
            hasTarget = false;
        }
    }

    private void AcquireTarget()
    {
        offset = Random.onUnitSphere;
        offset = new Vector3(offset.x, Mathf.Abs(offset.y), offset.z);
        offset *= hoverRange;
        hasTarget = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Layer.ground || collision.gameObject.layer == Layer.enemy)
        {
            AcquireTarget();
        }
    }
}
