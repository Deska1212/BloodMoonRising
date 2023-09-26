using System;
using UnityEngine;
using Random = System.Random;

public class Crossbow : RangedWeapon
{
	[SerializeField] private Animator meshAnimator; // For animating the mesh itself e.g. the crossbows wings
	[SerializeField] private AudioClip[] fireSounds;
	[SerializeField] private AudioClip[] reloadSounds;
	[SerializeField] private ParticleSystem fireParticle;

	// Events
	public static Action OnCrossbowFire;

	/// <summary>
	/// Crossbow constructor
	/// </summary>
	public Crossbow()
	{
		weaponName = "Crossbow";
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		animator = GetComponentInChildren<Animator>();
		ammoController = GetComponentInParent<AmmoController>();
		Debug.Log(ammoController);
		weaponsController = GetComponentInParent<WeaponsController>(); 
		ChangeState(new CrossbowEquippingState());
	}

	protected override void Update()
	{
		base.Update();
		CrosshairCorrection();
	}

	public override void Fire()
	{
		OnFire(); // Plays string animation
		base.Fire();
	}

	#region Audio

	private void PlayFireAudio()
	{
		int rand = UnityEngine.Random.Range(0, fireSounds.Length);
		AudioClip clip = fireSounds[rand];
		audioSource.PlayOneShot(clip);
	}
	
	private void PlayReloadAudio()
	{
		int rand = UnityEngine.Random.Range(0, reloadSounds.Length);
		AudioClip clip = reloadSounds[rand];
		audioSource.PlayOneShot(clip);
	}

	#endregion

	public override void Equip()
	{
		ChangeState(new CrossbowEquippingState());
	}
	
	public override void Unequip()
	{
		ChangeState(new CrossbowUnequippingState());
	}

	public override void Ready()
	{
		ChangeState(new CrossbowReadyState());
	}
	
	public void OnFire()
	{
		PlayFireAudio();
		PlayFireParticleEffects();
		OnCrossbowFire?.Invoke();
		meshAnimator.SetBool("StringPulledBack", false);
	}

	public void OnReload()
	{
		PlayReloadAudio();
		meshAnimator.SetBool("StringPulledBack", true);
	}

	public void PullbackString()
	{
		meshAnimator.SetBool("StringPulledBack", true);
	}

	private void PlayFireParticleEffects()
	{
		fireParticle.Play();
	}
}
