using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour, IResettable
{
    [SerializeField] List<GameObject> loaded;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        foreach (GameObject obj in loaded)
        {
            obj.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != Layer.player)
        {
            return;
        }

        foreach (GameObject obj in loaded)
        {
            obj.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
