using System;
using UnityEngine;
using DentedPixel;


public class PointLightFadeOut : MonoBehaviour
{
	private Light pointLight;
	[SerializeField] private float fadeTime;

	[SerializeField] private Color startColor;
	[SerializeField] private Color endColor;

	private void Start()
	{
		pointLight = GetComponent<Light>();
		LeanTween.value(this.gameObject, FadeOutCallback, pointLight.intensity, 0, fadeTime);
		LeanTween.value(this.gameObject, ColorChangeCallback, startColor, endColor, fadeTime);
	}

	private void FadeOutCallback(float f)
	{
		pointLight.intensity = f;
	}

	private void ColorChangeCallback(Color c)
	{
		pointLight.color = c;
	}
}
