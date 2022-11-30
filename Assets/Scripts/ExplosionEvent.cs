using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionEvent : Event
{
    [SerializeField] private float radius;
    [SerializeField] private float duration;
    [SerializeField] private float damageToPlayer;
    [SerializeField] private float damageToEnemies;
    [SerializeField] private float force;
    [SerializeField] Transform visual;

    public override void Spawn()
    {
        base.Spawn();
        foreach(Collider entity in Physics.OverlapSphere(transform.position, radius))
        {
            if (entity.gameObject.layer == Layer.player)
            {
                var player = entity.GetComponent<PlayerController>();
                if (!player)
                {
                    return;
                }
                player.AddExplosionForce(force, transform.position);
            }

            if (entity.gameObject.layer == Layer.enemy)
            {
                var enemy = entity.GetComponent<Enemy>();
                if (!enemy)
                {
                    return;
                }
                enemy.Damage(damageToEnemies);
                enemy.ExplosionForce(force, transform.position);
            }
        }
        visual.localScale = Vector3.one * radius * 1.5f;
        visual.DOScale(Vector3.zero, duration).SetEase(Ease.OutCubic);
    }
}
