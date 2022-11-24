using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "SoundManager", menuName = "ScriptableObjects/SoundManager", order = 1)]
public class AudioManager : ScriptableObject
{
    public static AudioManager Instance { get; private set; }
    public List<Sound> sounds = new List<Sound>();

    private Dictionary<string, AudioClip> clips;


    public AudioManager()
    {
        Instance = this;
    }

    public static void Initialize()
    {
        Instance.clips = new Dictionary<string, AudioClip>();
        foreach (Sound sound in Instance.sounds)
        {
            Instance.clips.Add(sound.name, sound.clip);
        }
        Debug.Log("AudioManager initialized with " + Instance.clips.Count + " sounds.");
    }


    public AudioClip GetClip(string name)
    {
        return clips[name];
    }
}