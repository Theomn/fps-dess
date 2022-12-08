using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected Enemy me;
    protected Transform player;
    protected float distanceToPlayer;
    /// <summary>
    /// Will always be true if there are no TrackingBehaviour on that same object.
    /// </summary>
    protected bool isFacingPlayer;
    private TrackingBehaviour tracking;

    protected virtual void Awake()
    {
        me = GetComponentInParent<Enemy>();
        tracking = GetComponent<TrackingBehaviour>();
    }

    protected virtual void Start()
    {
        player = PlayerController.instance.transform;
    }
    
    protected virtual void Update()
    {
        distanceToPlayer = me.GetDistanceToPlayer();
        isFacingPlayer = tracking ? tracking.IsFacingPlayer() : true;
    }
}
