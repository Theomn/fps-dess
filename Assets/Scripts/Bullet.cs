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

    public void Initialize(Vector3 direction)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.forward * speed * Time.deltaTime, float.MaxValue);
    }

    private void Despawn()
    {

    }
}
