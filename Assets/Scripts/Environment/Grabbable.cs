using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [SerializeField] private Consummable consummable;

    public void Grab()
    {
        ItemBelt.Instance.AddConsummable(consummable);
        gameObject.SetActive(false);
    }
}
