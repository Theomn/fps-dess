using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public struct Text
{
    public string key;
    public string english;
    public float time;
}

[CreateAssetMenu(fileName = "LocalizationManager", menuName = "Scriptable Objects/Localization", order = 2)]
public class Localization : ScriptableObject
{
    public List<Text> localization;

    public Text GetText(string key)
    {
        return localization.Find(t => t.key.Equals(key));
    }
}
