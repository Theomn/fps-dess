using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    private float deathTorqueIntensity = 50f;
    private float deathGravityIntensity = 30f;
    private float deathPunchIntensity = 200f;
    private Rigidbody rb;
    private float initialDrag;
    private float initialAngularDrag;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        initialDrag = rb.drag;
        initialAngularDrag = rb.angularDrag;
    }

    public override void Reset()
    {
        base.Reset();
        rb.drag = initialDrag;
        rb.angularDrag = initialAngularDrag;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (flaggedForDestroy)
        {
            rb.AddForce(Vector3.down * deathGravityIntensity, ForceMode.Acceleration);
        }
    }

    protected override void FlagForDestroy()
    {
        base.FlagForDestroy();
        rb.angularDrag = 0f;
        rb.drag = 0.5f;
        rb.AddTorque(new Vector3(r(), r(), r()), ForceMode.Impulse);
        var punch = Random.onUnitSphere * deathPunchIntensity;
        punch = new Vector3(punch.x, Mathf.Abs(punch.y), punch.z);
        rb.AddForce(punch, ForceMode.Impulse);
    }

    public override void ExplosionForce(float explosionForce, Vector3 explosionPosition)
    {
        base.ExplosionForce(explosionForce, explosionPosition);
        var direction = (transform.position - explosionPosition).normalized;
        rb.AddForce(direction * explosionForce, ForceMode.Impulse);
    }

    private float r()
    {
        return Random.Range(-deathTorqueIntensity, deathTorqueIntensity);
    }
}
