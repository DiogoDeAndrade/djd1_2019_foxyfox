using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMng : MonoBehaviour
{
    static public SoundMng instance;

    List<AudioSource> audioSources;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this);

        audioSources = new List<AudioSource>();
    }

    public void PlaySound(AudioClip sound, float volume = 1.0f, float frequency = 1.0f)
    {
        AudioSource audioSource = NewSoundObject();
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.pitch = frequency;
        audioSource.Play();
    }

    AudioSource NewSoundObject()
    {
        foreach (AudioSource audio in audioSources)
        {
            if (!audio.isPlaying)
            {
                return audio;
            }
        }

        GameObject  gObject = new GameObject();
        gObject.transform.parent = transform;
        gObject.name = "SoundFX";
        AudioSource audioSource = gObject.AddComponent<AudioSource>();

        audioSources.Add(audioSource);

        return audioSource;
    }
}
