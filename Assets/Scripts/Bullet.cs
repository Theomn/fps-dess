using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private int pierceCount;
    [SerializeField] private float lifetime;

    private float lifetimeTimer;

    public void Initialize()
    {
        lifetimeTimer = lifetime;
    }


    // Update is called once per frame
    void Update()
    {
        if (lifetimeTimer > 0f)
        {
            lifetimeTimer -= Time.deltaTime;
            if (lifetimeTimer <= 0)
            {
                Despawn();
            }
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + transform.forward * speed * Time.deltaTime, float.MaxValue);
    }

    private void Despawn()
    {
        LeanPool.Despawn(this);
    }
}