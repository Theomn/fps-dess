using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int id;

    void Start()
    {
        ProgressionManager.Instance.Register(id, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.player)
        {
            ProgressionManager.Instance.ActivateCheckpoint(id);
        }
    }
}
