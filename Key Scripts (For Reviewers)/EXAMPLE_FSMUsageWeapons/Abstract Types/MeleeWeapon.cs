using System;
using UnityEngine;


public class MeleeWeapon : Weapon
{
	// Melee weapon specific
	[SerializeField] protected Transform hitPoint; // This is the centre point of the damage area
	[SerializeField] protected float hitRadius;
	[SerializeField] protected float damage;
	
	[SerializeField] private AudioClip[] slashSounds;
	[SerializeField] protected AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	/// <summary>
	/// This function is called by an animation event at the peak of an animation.
	/// It pulls all the damageable gameobjects in an area and applied damage to them.
	/// </summary>
	private void ApplyDamageToArea()
	{
		// Overlap sphere and grab all the game objects in the area
		Collider[] colliders = Physics.OverlapSphere(hitPoint.position, hitRadius);
		
		// Grab the IDamageable component of each game object and
		// apply damage to each of those components.

		Debug.Log("Applied " + damage + " damage to: " + colliders.Length + " objects"); // TODO: Remove debug
		
		foreach (var col in colliders)
		{
			bool exists = col.TryGetComponent<IDamageable>(out IDamageable damageable);

			if (exists)
			{
				damageable.TakeDamage(damage);
			}
		}
	}

	

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(hitPoint.position, hitRadius);
	}
	
	/// <summary>
	/// Called by animator event
	/// </summary>
	public void OnSwing()
	{
		ApplyDamageToArea();
		PlaySlashAudio();
	}
	
	protected void PlaySlashAudio()
	{
		int rand = UnityEngine.Random.Range(0, slashSounds.Length);
		AudioClip clip = slashSounds[rand];
		audioSource.PlayOneShot(clip);
	}
}
