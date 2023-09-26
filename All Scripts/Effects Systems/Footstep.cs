using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for randomising the pitch and volume slightly
/// </summary>
public class Footstep : MonoBehaviour
{
    [SerializeField] private float pitchVariance;
    [SerializeField] private float volumeVariance;
    
    void Start()
    {
        // Get the AudioSource component, randomize pitch and volume slightly, and play footstep audio
        AudioSource source = GetComponent<AudioSource>();

        source.pitch += Random.Range(-pitchVariance, pitchVariance);
        source.volume += Random.Range(-volumeVariance, volumeVariance);

        source.Play();
    }
}
