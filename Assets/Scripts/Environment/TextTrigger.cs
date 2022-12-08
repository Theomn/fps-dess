using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private string key;

    private Text text;

    private void Awake()
    {
        text = Localization.instance.GetText(key);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Layer.player)
        {
            HUDController.Instance.DisplayText(text.english, text.time);
            gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        gameObject.SetActive(true);
    }
}
