using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Event : MonoBehaviour
{
    [Tooltip("How long the object will stay alive before being destroyed")]
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

    public virtual void Spawn()
    {
        lifetimeTimer = lifetime;
    }

    protected virtual void Despawn()
    {
        LeanPool.Despawn(this);
    }
}
