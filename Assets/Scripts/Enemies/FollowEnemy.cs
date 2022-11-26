using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{
   
    private Transform player;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followRange;
    [Tooltip("Minimum distance from this enemy to player")]
    [SerializeField] private float hoverRange;

    private bool hasTarget;
    private Vector3 targetPosition;
    private Rigidbody rb;



    new void Start()
    {
        base.Start();
        player = PlayerController.Instance.transform;
        rb = GetComponent<Rigidbody>();



    }

    new void Update()
    {
        base.Update();
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if(distanceToPlayer <= followRange && facingPlayer) 
        {
            if (!hasTarget)
            {
                targetPosition = Random.onUnitSphere;
                targetPosition = new Vector3(targetPosition.x, Mathf.Abs(targetPosition.y), targetPosition.z);
                targetPosition *= hoverRange;
                hasTarget = true;
            }

            /// transform.position = Vector3.MoveTowards(transform.position,player.position + targetPosition, followSpeed * Time.deltaTime);
            var moveDirection = ((player.position + targetPosition) - transform.position).normalized * followSpeed * Time.deltaTime;
            // rb.MovePosition(moveDirection + transform.position);
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.position + targetPosition, followSpeed * Time.deltaTime));
            
            
        }
        else
        {
            hasTarget = false;
        }

        
    }
}
