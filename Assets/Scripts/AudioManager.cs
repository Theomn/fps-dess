using System;
using UnityEngine;
using System.Collections.Generic;


public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public List<SoundBank> soundBanks;

    private Dictionary<string, AudioClip> clips;

    private AudioSource source;


    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
        clips = new Dictionary<string, AudioClip>();

        foreach (SoundBank bank in soundBanks)
        {
            foreach (Sound sound in bank.sounds)
            {
                if (!clips.TryAdd(sound.name, sound.clip))
                {
                    Debug.LogWarning("Sounds with name \"" + sound.name + "\" already exists in SoundBanks.");
                }
            }
        }
        Debug.Log("AudioManager initialized with " + clips.Count + " sounds.");
    }

    public AudioClip GetClip(string name)
    {
        clips.TryGetValue(name, out var clip);
        if (!clip)
        {
            Debug.LogWarning("Sound with name \"" + name + "\" does not exist in SoundBank.");
        }
        return clip;
    }

    public void PlaySoundAtPosition(string soundName, Vector3 position)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            return;
        }
        var clip = GetClip(soundName);
        if (!clip)
        {
            return;
        }
        transform.position = position;
        source.PlayOneShot(clip);
    }
}