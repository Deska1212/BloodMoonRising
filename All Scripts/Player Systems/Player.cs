using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;




public class Player : Entity
{
	[SerializeField] private int potionCount;
	private float stamina;
	
	[Header("Damage FX")]
	
	// Damage FX
	// TODO: Remove serialize fields after testing
	[SerializeField] private Volume volume;
	[SerializeField] private Vignette vignette;
	[SerializeField] private float takeDamageVignetteIntensity;
	[SerializeField] private float defaultVignetteIntensity;
	[SerializeField] private float vignetteFadeSpeed;
	
	

	// Events
	public static Action<int> OnPotionCountChanged; // Potion changed event, invoked when we have a change in potion count (in property)
	public static Action<float> OnHealthChanged;
	public static Action<float> OnStaminaChanged;
	
	
	
	[SerializeField] private float maxStamina;

	#region PlayerProperties

	public int PotionCount
	{
		get { return potionCount;  }
		set
		{
			potionCount = value;
			OnPotionCountChanged?.Invoke(potionCount); // Invoke our OnPotionCountChanged event and pass in the potion count for listeners
		}
	}

	public override float Health
	{
		get
		{
			return base.Health;
		}

		set
		{
			base.Health = value;
			OnHealthChanged?.Invoke(base.Health);
		}
	}

	public float Stamina
	{
		get
		{
			return stamina;
		}
		set
		{
			stamina = value;
			OnStaminaChanged?.Invoke(stamina);
		}
	}

	public float MaxStamina
	{
		get { return maxStamina; }
	}

	#endregion

	private void Start()
	{
		stamina = 100f;
		volume = GameObject.FindWithTag("Volume").GetComponent<Volume>();
		bool val = volume.profile.TryGet<Vignette>(out vignette);

		defaultVignetteIntensity = vignette.intensity.value;
	}

	private void Update()
	{
		// TODO: Delet this
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// TODO: TEMP TEMP TEMP   
			// Quit the game if we are in a build 
			#if !UNITY_EDITOR
				SceneManage.instance.LoadScene(1);
			#endif
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			DrinkPotion();
		}
	}

	private void DrinkPotion()
	{
		if (PotionCount > 0)
		{
			// We can drink a potion
			PotionCount--;
			
			//TODO: Do health logic in another method
			
			
			// Play potion drinking sound
			AudioManager.instance.Play("Potion");
			
			// Right now we are just refilling to max health
			Health = 100f;
		}
		else
		{
			// Player doesn't have any potions but just tried to drink one tell them their a dickhead
			AudioManager.instance.Play("NoPotion");
			
			// Still need to reference the potion count to fire the event
			PotionCount = PotionCount; // This is kinda yucky but its just so the event fires
		}
	}


	public override void TakeDamage(float dmg)
	{
		// Player damage
		Health -= dmg;
		
		// Take damage post fx
		DamageIndicationFX();
		
		// Camera shake
		GetComponent<CameraController>().DamageShake(dmg);
	}

	private void DamageIndicationFX()
	{
		
		
		// Tween the vignette to a higher intensity then tween it back out
		vignette.intensity.value = takeDamageVignetteIntensity;
		vignette.color.value = Color.red;
		
		
		// Start tweening intensity back
		LeanTween.value(this.gameObject, (float f ) => vignette.intensity.value = f,
			vignette.intensity.value, defaultVignetteIntensity, vignetteFadeSpeed);
		
		// Tween color back
		LeanTween.value(this.gameObject, (Color c) => vignette.color.value = c,
			vignette.color.value, Color.black, vignetteFadeSpeed);
	}


	public override void Die()
	{
		// Player Death
		Debug.Log("Player died :P");
		
		// Disable Input
		InputManager.Instance.enabled = false;
		GetComponent<CameraController>().enabled = false;
		
		// Scene Reset
		SceneManage.instance.LoadScene(3);
	}
}
