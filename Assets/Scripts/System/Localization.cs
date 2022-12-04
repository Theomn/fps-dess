using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Text
{
    public string id;
    public string english;
    public float time;
}

[CreateAssetMenu(fileName = "Localization", menuName = "Scriptable Objects/Localization", order = 1)]
public class Localization : ScriptableObject
{
    public List<Text> localization;
}
