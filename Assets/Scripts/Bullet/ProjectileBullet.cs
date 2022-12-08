using UnityEngine;
using Lean.Pool;

public class ProjectileBullet : Bullet
{
    [SerializeField] private GameObject visual;
    private Rigidbody rb;
    private int pierceCount;
    protected TrailRenderer trail;
    protected Transform player;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Start()
    {
        player = PlayerController.Instance.transform;
    }

    public override void Spawn(BulletData data)
    {
        base.Spawn(data);
        trail?.Clear();

        pierceCount = 0;
        GetComponent<Collider>().enabled = true;
        visual.SetActive(true);
        rb.velocity = transform.forward * data.speed;
    }

    protected virtual void FixedUpdate()
    {
        if (flaggedForDespawn)
        {
            return;
        }

        // Track player
        if (data.trackingSpeed > 0)
        {
            var step = data.trackingSpeed * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            rb.MovePosition(Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * data.speed * Time.fixedDeltaTime, float.MaxValue));
        }
        

        // Apply gravity
        if (data.gravity != 0)
        {
            rb.AddForce(Vector3.down * data.gravity * 50 * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    protected override void FlagForDespawn()
    {
        base.FlagForDespawn();
        GetComponent<Collider>().enabled = false;
        visual.SetActive(false);

        //Make sure the bullet is exactly on the contact point
        /*Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Layer.ground + Layer.enemy + Layer.player);
        if (hit.collider)
        {
            transform.position = hit.point;
        }*/
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