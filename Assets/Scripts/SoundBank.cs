using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "SoundBank", menuName = "Scriptable Objects/Sound Bank", order = 1)]
public class SoundBank : ScriptableObject
{
    public List<Sound> sounds;
}
