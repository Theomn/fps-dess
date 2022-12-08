using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [SerializeField] private Consummable consummable;

    public void Grab()
    {
        ItemBelt.instance.AddConsummable(consummable);
        gameObject.SetActive(false);
    }
}
