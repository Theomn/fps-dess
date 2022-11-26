using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{
   
    private Transform player;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followRange;


    new void Start()
    {
        base.Start();
        player = PlayerController.Instance.transform;
       
    }

    new void Update()
    {
        base.Update();
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if(distanceToPlayer <= followRange && targetAcquired == true ) 
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }
}
