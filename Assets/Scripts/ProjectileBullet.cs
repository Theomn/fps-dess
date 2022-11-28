using UnityEngine;
using Lean.Pool;

public class ProjectileBullet : Bullet
{
    private Rigidbody rb;
    private TrailRenderer trail;
    private int pierceCount;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    public override void Spawn(BulletData data)
    {
        base.Spawn(data);
        pierceCount = 0;
        trail?.Clear();
    }
    
    protected virtual void FixedUpdate()
    {
        // Travel forward
        rb.MovePosition(Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * data.speed * Time.fixedDeltaTime, float.MaxValue));

        // Apply gravity
        if (data.gravity != 0)
        {
            rb.AddForce(Vector3.down * data.gravity * 10 * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    protected override void Despawn()
    {
        //Make sure the bullet is exactly on the contact point
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Layer.ground + Layer.enemy + Layer.player);
        if (hit.collider)
        {
            transform.position = hit.point;
        }
        if (data.endEvent)
        {
            var evt = LeanPool.Spawn(data.endEvent);
            evt.transform.position = transform.position;
            evt.Spawn();
        }
        base.Despawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        int layer = Deliver(other);
        if (layer == Layer.player || layer == Layer.enemy)
        {
            pierceCount++;
            if (pierceCount >= data.maxPierceCount)
            {
                Despawn();
            }
        }
        else if (layer == Layer.ground)
        {
            if (data.destroyOnGroundContact)
            {
                Despawn();
            }
        }
    }
}