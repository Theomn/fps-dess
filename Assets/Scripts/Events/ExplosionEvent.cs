using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionEvent : Event
{
    [Header("Explosion Stats")]
    [SerializeField] private float radius;
    [SerializeField] private float damageToPlayer;
    [SerializeField] private float damageToEnemies;
    [SerializeField] private float force;

    [Header("FX")]
    [SerializeField] Transform visual;
    [SerializeField] private float duration;
    [SerializeField] private string explosionSound;


    
    private List<Enemy> damagedEnemies = new List<Enemy>();

    public override void Spawn()
    {
        base.Spawn();
        foreach(Collider entity in Physics.OverlapSphere(transform.position, radius))
        {
            if (entity.gameObject.layer == Layer.player)
            {
                var player = entity.GetComponent<PlayerController>();
                if (player)
                {
                    player.AddExplosionForce(force, transform.position);
                    player.Damage(damageToPlayer);
                }
            }

            if (entity.gameObject.layer == Layer.enemy)
            {
                var enemy = entity.GetComponentInParent<Enemy>();
                if (enemy && !damagedEnemies.Contains(enemy))
                {
                    damagedEnemies.Add(enemy);
                    enemy.Damage(damageToEnemies);
                    enemy.ExplosionForce(force, transform.position);
                }
            }
            AudioManager.instance.PlaySoundAtPosition(explosionSound, transform.position);
        }
        damagedEnemies.Clear();
        visual.localScale = Vector3.one * radius * 2f;
        visual.DOScale(Vector3.zero, duration).SetEase(Ease.OutCubic);
    }
}
