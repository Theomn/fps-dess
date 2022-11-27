using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{
    [Header("Follow Behaviour")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float followRange;
    [Tooltip("Minimum distance from this enemy to player")]
    [SerializeField] private float hoverRange;

    private bool hasTarget;
    private Vector3 offset;
    private Rigidbody rb;



    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    new void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if(distanceToPlayer <= followRange && facingPlayer) 
        {
            if (!hasTarget)
            {
                AcquireTarget();
            }

            var targetPosition = (player.position + offset);
            var moveForce = Vector3.ClampMagnitude((targetPosition - transform.position), 1f) * acceleration;
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(moveForce);
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
        if (collision.gameObject.layer == Layer.groundLayer || collision.gameObject.layer == Layer.enemyLayer)
        {
            AcquireTarget();
        }
    }
}
