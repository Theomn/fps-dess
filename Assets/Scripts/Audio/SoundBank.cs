using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public struct Sound
{
    public string name;
    public List<AudioClip> clips;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 0.1f)] public float pitchRandom;
    [Range(0f, 1f)] public float spatialisation;
}

[CreateAssetMenu(fileName = "SoundBank", menuName = "Scriptable Objects/Sound Bank", order = 1)]
public class SoundBank : ScriptableObject
{
    public List<Sound> sounds;

}
