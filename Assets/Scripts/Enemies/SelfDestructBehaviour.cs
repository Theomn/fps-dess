using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructBehaviour : EnemyBehaviour
{
    [SerializeField] private float range;


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        foreach (Collider entity in Physics.OverlapSphere(transform.position, range))
        {
            if (entity.gameObject.layer == Layer.player)
            {
                var player = entity.GetComponent<PlayerController>();
                if (player)
                {
                    me.Kill();
                }
            }
        }
    }
}
