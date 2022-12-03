using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : EnemyBehaviour
{
    [SerializeField] private float fireRange;
    [Tooltip("If true, will only fire when roughly facing player. Needs a TrackingBehaviour component on the same object.")]
    [SerializeField] private bool firesWhenFacingPlayer;
    [SerializeField] private EnemyGun gun;
    [SerializeField] private GunDeprecated gunD;


    protected override void Update()
    {
        base.Update();
        bool canFire = firesWhenFacingPlayer ? isFacingPlayer : true;
        if (distanceToPlayer <= fireRange && canFire)
        {
            gunD?.Fire();
            gun?.Fire();
        }
    }
}
