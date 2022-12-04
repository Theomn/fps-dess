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
        foreach (GameObject obj in loaded)
        {
            obj.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
