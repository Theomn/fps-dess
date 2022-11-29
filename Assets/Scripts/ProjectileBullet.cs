using UnityEngine;
using Lean.Pool;

public class ProjectileBullet : Bullet
{
    [SerializeField] private GameObject visual;
    private Rigidbody rb;
    private int pierceCount;
    protected TrailRenderer trail;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();

    }

    public override void Spawn(BulletData data)
    {
        base.Spawn(data);
        trail?.Clear();

        pierceCount = 0;
        GetComponent<Collider>().enabled = true;
        visual.SetActive(true);
    }

    protected virtual void FixedUpdate()
    {
        if (flaggedForDespawn)
        {
            return;
        }
        // Travel forward
        rb.MovePosition(Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * data.speed * Time.fixedDeltaTime, float.MaxValue));

        // Apply gravity
        if (data.gravity != 0)
        {
            rb.AddForce(Vector3.down * data.gravity * 10 * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    protected override void FlagForDespawn()
    {
        base.FlagForDespawn();
        GetComponent<Collider>().enabled = false;
        visual.SetActive(false);

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
    }

    private void OnTriggerEnter(Collider other)
    {
        int layer = Deliver(other);
        if (layer == Layer.player || layer == Layer.enemy)
        {
            pierceCount++;
            if (pierceCount >= data.maxPierceCount)
            {
                FlagForDespawn();
            }
        }
        else if (layer == Layer.ground)
        {
            if (data.destroyOnGroundContact)
            {
                FlagForDespawn();
            }
        }
    }
}