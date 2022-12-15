
using UnityEngine;
using System.Collections.Generic;


public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public List<SoundBank> soundBanks;

    private Dictionary<string, Sound> clips;

    private AudioSource source;


    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
        clips = new Dictionary<string, Sound>();

        foreach (SoundBank bank in soundBanks)
        {
            foreach (Sound sound in bank.sounds)
            {
                if (!clips.TryAdd(sound.name, sound))
                {
                    Debug.LogWarning("Sounds with name \"" + sound.name + "\" already exists in SoundBanks.");
                }
            }
        }
        Debug.Log("AudioManager initialized with " + clips.Count + " sounds.");
    }

    public AudioClip GetClip(string name)
    {
        if (!clips.TryGetValue(name, out var sound))
        {
            Debug.LogWarning("Sound with name \"" + name + "\" does not exist in SoundBank.");
        }
        return sound.clips[Random.Range(0, sound.clips.Count)];
    }

    public void PlaySoundAtPosition(string soundName, Vector3 position)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            return;
        }
        if (!clips.TryGetValue(soundName, out var sound))
        {
            Debug.LogWarning("Sound with name \"" + name + "\" does not exist in SoundBank.");
            return;
        }
        //parametres dans SoundBanks.cs
        transform.position = position;
        source.pitch = Random.Range(-sound.pitchRandom, sound.pitchRandom) + 1f;

        source.PlayOneShot(sound.clips[Random.Range(0, sound.clips.Count)], sound.volume);
    }

    public void PlaySound(string soundName, AudioSource source)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            return;
        }
        if (!clips.TryGetValue(soundName, out var sound))
        {
            Debug.LogWarning("Sound with name \"" + name + "\" does not exist in SoundBank.");
            return;
        }
        //parametres dans SoundBanks.cs
        source.pitch = Random.Range(-sound.pitchRandom, sound.pitchRandom) + 1f;
        
        source.PlayOneShot(sound.clips[Random.Range(0, sound.clips.Count)], sound.volume);
    }
}