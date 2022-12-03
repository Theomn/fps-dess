using UnityEngine;
using Lean.Pool;
using DG.Tweening;

public class RaycastBullet : Bullet
{
    [SerializeField] private LayerMask raycastLayerMask;
    private LineRenderer line;
    private ParticleSystem particles;
    private Vector3[] positions = new Vector3[2];
    private float initialWidth;
    private bool moveNextFrame;
   

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        particles = GetComponent<ParticleSystem>();
        initialWidth = line.startWidth;
    }

    protected override void Update()
    {
        base.Update();
        if (moveNextFrame)
        {
            transform.position = positions[1];
            moveNextFrame = false;
        }
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
        FlagForDespawn();

        line.SetPositions(positions);
        line.startWidth = initialWidth;
        moveNextFrame = true;
        if (particles)
        {
            particles.Clear();
        }
        DOTween.To(() => line.startWidth, x => line.startWidth = x, 0, despawnOffset).SetEase(Ease.OutExpo);
    }
}
