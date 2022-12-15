using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerGun : Gun
{
    [Header("Energy")]
    [SerializeField] private float maxEnergy;
    [Tooltip("How much energy regenerates per second.")]
    [SerializeField] private float energyRechargeRate;
    [Tooltip("Cost of one trigger pull.")]
    [SerializeField] private float energyCost;

    [Header("Nozzle")]
    [SerializeField] private Transform nozzle;
    [Tooltip("Spheres that flashs at the tip of the nozzle when gun is fired.")]
    [SerializeField] private Transform nozzleFlash;

    [Header("Recoil")]
    [SerializeField] private float rotationRecoil;
    [SerializeField] private float positionRecoil;
    [SerializeField] private float recoilDuration;

    [Header("Misc")]
    [SerializeField] public int id;
    [SerializeField] public bool canZoom;
    [SerializeField] public bool isConsummable;
    [SerializeField] public bool cannotSwitch;
    [SerializeField] public GameObject visual;

    private float energy;
    private Quaternion initialRotation;
    private Vector3 initialPosition;


    protected override void Awake()
    {
        base.Awake();
        energy = maxEnergy;
        activeNozzle = nozzle;
        activeNozzleFlash = nozzleFlash;
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
    }

    protected override void Update()
    {
        base.Update();
        if (energy < maxEnergy)
        {
            energy += energyRechargeRate * Time.deltaTime;
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
        }
    }

    public override bool Fire()
    {
        if(energy < energyCost)
        {
            return false;
        }

        if (!base.Fire())
        {
            return false;
        }

        energy -= energyCost;


        if (recoilDuration > 0)
        {
            transform.DOKill();
            transform.localRotation = initialRotation;
            transform.localPosition = initialPosition;
            transform.DOPunchRotation(Vector3.left * rotationRecoil, recoilDuration, 0, 0);
            transform.DOPunchPosition(Vector3.back * positionRecoil, recoilDuration, 0, 0);
        }
        return true;
    }

    protected override void ArmNozzle()
    {
        base.ArmNozzle();
        activeNozzle.LookAt(CameraController.instance.GetCrosshairTarget());
    }

    public void Holster()
    {
        visual.SetActive(false);
    }

    public void Unholster()
    {
        visual.SetActive(true);
        HUDController.instance.SetMaxEnergy(maxEnergy);
    }

    public float GetEnergy()
    {
        return energy;
    }
}
