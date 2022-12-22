using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip postRoll;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.PlayOneShot(postRoll);
        musicSource.PlayScheduled(musicSource.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}