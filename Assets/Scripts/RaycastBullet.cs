using UnityEngine;
using Lean.Pool;

public class RaycastBullet : Bullet
{
    [SerializeField] private LayerMask raycastLayerMask;
    private LineRenderer line;
    private Vector3[] positions = new Vector3[2];

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public override void Spawn(BulletData data)
    {
        base.Spawn(data);
        positions[0] = transform.position;
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, data.maxRange, raycastLayerMask);
        if (hit.collider)
        {
            Deliver(hit.collider);
            positions[1] = hit.point;
            if (data.endEvent)
            {
                var evt = LeanPool.Spawn(data.endEvent);
                evt.transform.position = hit.point;
                evt.Spawn();
            }
        }
        else
        {
            positions[1] = transform.position + transform.TransformDirection(Vector3.forward * data.maxRange);
        }
        line.SetPositions(positions);
    }
}
