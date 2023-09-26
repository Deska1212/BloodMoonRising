using UnityEngine.Audio;
using System;
using UnityEngine;

// For 2D audio, UI, music, zings etc
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Provides an instance of Audiomanager to compare if another is in scene
    public static AudioManager instance;
   
    void Awake()
    {

        instance = this;
        

        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clip;

            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
            s.Source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        //in line below name == the name of the sound in the inspector once a sound has been added
        // Run this function within another script by - FindObjectOfType<AudioManager>().play("name");
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            // Warning that sound with name not found
            Debug.LogWarning("Sound: " + name + " not found!");
            // no sound will be played stopping errors 
            return;
        }

        s.Source.Play();
    }
}
