using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour <T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this as T;
        }
    }
}
