using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    [SerializeField] protected float lifetime;
    protected float lifetimeTimer;

    protected virtual void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0)
        {
            Despawn();
        }
    }

    public abstract void Spawn();
    public abstract void Despawn();
}
