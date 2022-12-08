using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    [Header("Nozzle")]
    [Tooltip("Position and direction where bullets spawn.")]
    [SerializeField] private List<Transform> nozzles;
    [Tooltip("If true, the nozzle will rotate towards the player every time the gun is fired. " +
        "Set to false for guns equipped on player.")]
    [SerializeField] private bool fireAtPlayer;

    private int activeNozzleId;

    protected override void ArmNozzle()
    {
        base.ArmNozzle();
        if (activeNozzleId >= nozzles.Count)
        {
            activeNozzleId = 0;
        }
        activeNozzle = nozzles[activeNozzleId];
        activeNozzleFlash = nozzles[activeNozzleId];
        activeNozzleId++;

        if (fireAtPlayer)
        {
            activeNozzle.LookAt(PlayerController.instance.transform.position);
        }
    }
}
