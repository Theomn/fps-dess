using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private string textId;
    [SerializeField] private Localization localizationFile;

    private Text text;

    private void Awake()
    {
        text = localizationFile.localization.Find(text => text.id.Equals(textId));
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
