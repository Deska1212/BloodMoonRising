using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Class used for every lamp post to trigger the
/// snuff out behaviour
/// </summary>
public class LampPost : MonoBehaviour
{

    [SerializeField] private bool active;
    [Tooltip("The time it takes for the light to snuff out")]
    [SerializeField] private float lightSnuffTime;
    
    [Header("Values to be assigned in inspector")]
    [SerializeField] private Light light;
    [SerializeField] private Renderer lightRenderer;

    [SerializeField] private float snuffedOutEmissionIntensity;
    [SerializeField] private float snuffedOutLightIntensity;

    [SerializeField] private Color emissionColor;
    [SerializeField] private float activeEmissionIntensity;
    [SerializeField] private float activeLightIntensity;
    
    [SerializeField] private float emissivePerlin;
    [SerializeField] private float lightPerlin;
    [SerializeField] private float perlinFrequencyModifier;

    [SerializeField] private AnimationCurve easeCurve;


    private void Awake()
    {
        light = GetComponentInChildren<Light>();
        ApplyDefaultValues();
    }

    private void ApplyDefaultValues()
    {
        lightRenderer.material.SetColor("_EmissionColor", emissionColor * activeEmissionIntensity);
        light.intensity = activeLightIntensity;
    }

    private void Update()
    {
        EmissionNoise();
        LightNoise();
    }

    

    private void EmissionNoise()
    {
        // If we are active apply a perlin noise to the emission value
        if (active)
        {
            float perlin = Perlin(emissivePerlin);
            lightRenderer.material.SetColor("_EmissionColor", emissionColor * (activeEmissionIntensity + perlin));
        }

    }


    private void LightNoise()
    {
        // If we are active apply the same perlin noise to light intensity value
        if (active)
        {
            float perlin = Perlin(lightPerlin);
            light.intensity = activeLightIntensity + perlin;
        }
    }
    
    /// <summary>
    /// Helper for perlin stuff
    /// </summary>
    private float Perlin(float multiplier)
    {
        return Mathf.PerlinNoise(Time.timeSinceLevelLoad / perlinFrequencyModifier, Time.timeSinceLevelLoad / perlinFrequencyModifier) * multiplier;
    }
    

    /// <summary>
    /// This function gets called when the player interacts with an ammo box associated with this lamp post
    /// or walks into the trigger area
    /// </summary>
    public void SnuffOut()
    {
        if (active)
        {
            // Tweens down light intensity and disables the light and changes material when complete - We can easily grab our current light intensity
            LeanTween.value(gameObject, UpdateLightIntensity, light.intensity, snuffedOutLightIntensity, lightSnuffTime).setOnComplete(DisableLampPost).setEase(easeCurve);
        
            // Grab our current emissive intensity
            float intensity = GetCurrentEmissionMultiplier(lightRenderer.material);
            
            // Tween down material emission
            LeanTween.value(gameObject, UpdateMaterialEmission, intensity, snuffedOutEmissionIntensity, lightSnuffTime).setEase(easeCurve);;
            
            // Set light to be inactive
            active = false;
        }
    }

    private void UpdateMaterialEmission(float f)
    {
        lightRenderer.material.SetColor("_EmissionColor", emissionColor * f);
    }

    private void UpdateLightIntensity(float f)
    {
        light.intensity = f;
    }

    private void DisableLampPost()
    {
        // Change from emmisive(un-lit) to non-emmisive(lit) material
        // lightRenderer.sharedMaterial = nonEmissiveMaterial;
        
        // Disable the light
        light.enabled = false;
    }

    /// <summary>
    /// Gives the current intensity multiplier applied to the emission component of this material
    /// Assuming course color had greatest r,b,g component set to one (true in case of Color.Yellow)
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    private float GetCurrentEmissionMultiplier(Material mat)
    {
        var color = mat.GetColor("_EmissionColor");
        return Mathf.Max(color.r, color.g, color.b);
    }
}
