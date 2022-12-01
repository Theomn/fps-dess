using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] private float deathTorqueIntensity;
    [SerializeField] private float deathFallIntensity;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    protected override void FixedUpdate()
    {
        if (flaggedForDestroy)
        {
            rb.AddForce(Vector3.down * deathFallIntensity, ForceMode.Acceleration);
        }
    }
    protected override void FlagForDestroy()
    {
        base.FlagForDestroy();
        rb.angularDrag = 0f;
        rb.AddTorque(new Vector3(r(), r(), r()));
    }

    private float r()
    {
        return Random.Range(-deathTorqueIntensity, deathTorqueIntensity);
    }
}
