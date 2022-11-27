using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeZone : MonoBehaviour
{
    [SerializeField] private float playerDamagePerSecond;
    [SerializeField] private float enemyDamagePerSecond;

    private void OnTriggerStay(Collider other)
    {
        var layer = other.gameObject.layer;
        if (layer == Layer.playerLayer)
        {
            var player = other.GetComponent<PlayerController>();
            if (!player)
            {
                return;
            }
            player.Damage(playerDamagePerSecond * Time.fixedDeltaTime);
        }
        else if (layer == Layer.enemyLayer)
        {
            var enemy = other.GetComponent<Enemy>();
            if (!enemy)
            {
                return;
            }
            enemy.Damage(enemyDamagePerSecond * Time.fixedDeltaTime);
        }
    }
}
