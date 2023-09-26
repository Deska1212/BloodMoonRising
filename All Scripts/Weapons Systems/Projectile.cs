using System;
using System.Data.Common;
using UnityEngine;


public class Projectile : MonoBehaviour
{
	[SerializeField] private float baseVelocity;
	[SerializeField] private float baseDamage;

	private IProjectileDamageBehaviour projectileBehaviour; // Handles what happens to the OTHER object on collision
	private IProjectileDeathBehaviour projectileDeathBehaviour; // Handles what happens to THIS projectile when it collides with something
	
	// References
	private Rigidbody rb;
	
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		projectileBehaviour = GetComponent<IProjectileDamageBehaviour>();
		projectileDeathBehaviour = GetComponent<IProjectileDeathBehaviour>();
	}

	public void InitialiseProjectile(float velocityMultiplier, float damageMultiplier)
	{
		// Assign velocity from base velocity * ranged weapon velocity multiplier
		rb.velocity = transform.forward * (baseVelocity * velocityMultiplier);
		
		// Assign damage from base damage * ranged weapon damage multiplier
		baseDamage *= damageMultiplier;
	}

	private void OnCollisionEnter(Collision collision)
	{
		// Grab the IDamage component of the gameobject we
		// collided with if any and apply damage to it 
		bool exists = collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable);

		if (exists)
		{
			projectileBehaviour.ApplyDamage(damageable, baseDamage);
		}
		
		projectileDeathBehaviour.HandleCollision(this);
	}
}