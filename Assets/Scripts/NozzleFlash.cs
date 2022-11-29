using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NozzleFlash : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private float duration;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOPunchScale(Vector3.one * size, duration);
    }
}
