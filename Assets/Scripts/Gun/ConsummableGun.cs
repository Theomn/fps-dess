using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsummableGun : PlayerGun
{
    [Header("Consummable stats")]
    [SerializeField] private float invincibleTime;
    [SerializeField] private float healAmount;

    public override bool Fire()
    {
        if (!base.Fire())
        {
            return false;
        }

        if(healAmount > 0)
            PlayerController.instance.Heal(healAmount);
        if (invincibleTime > 0)
            PlayerController.instance.MakeInvincible(invincibleTime);
        return true;
    }
}
