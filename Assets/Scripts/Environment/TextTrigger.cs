using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private string key;

    private Text text;

    private void Start()
    {
        text = DataAccessor.instance.localization.GetText(key);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Layer.player)
        {
            HUDController.instance.DisplayText(text.english, text.time);
            gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        gameObject.SetActive(true);
    }
}
