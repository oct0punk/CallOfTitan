using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioSource source;
    [Range(0.0f, 3.0f)] public float volume;
    public bool loop;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            AudioSource s = gameObject.AddComponent<AudioSource>();
            s.clip = sound.clip;
            s.volume = sound.volume;
            s.loop = sound.loop;
            sound.source = s;
        }
    }

    public void Play(string clipName)
    {
        Sound s = Array.Find(sounds, s => s.name == clipName);
        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {clipName} not found");
        }
    }
}
