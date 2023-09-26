using System;
using UnityEngine;

public class RangedWeapon : Weapon
{
	

	// Ranged weapon relevant fields
	[SerializeField] protected float reloadTime; // Correlates to the reload animations length, we will probably just use that.
	[SerializeField] public bool loaded; // Is the crossbow currently loaded?
	[SerializeField] protected Transform projectileSpawn;
	[SerializeField] protected GameObject projectilePrefab;
	[SerializeField] protected float projectileVelocityMultiplier;
	[SerializeField] protected float projectileDamageMultiplier;

	[SerializeField] protected AudioSource audioSource;

	private float projSpawnDistanceThreshold = 1.5f; // Raycasts under this distance threshold don't cause the projectile spawn point to rotate to crosshair
	
	// References
	protected AmmoController ammoController; // Contains ammo info & behaviour - only ranged weapons need this

	public AmmoController GetAmmoController()
	{
		return ammoController;
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public virtual void Fire()
	{
		GameObject proj = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
		proj.GetComponent<Projectile>().InitialiseProjectile(projectileVelocityMultiplier, projectileDamageMultiplier);
	}

	/// <summary>
	/// If the player is aiming at distance far away from us, rotate the projectile spawn toward that point so bolts fire in that direction
	/// </summary>
	protected void CrosshairCorrection()
	{
		// TODO: Really unperformant -> Opportunity here to optimise
		if (Camera.main != null && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit))
		{
			if (hit.distance < projSpawnDistanceThreshold)
			{
				// If we are right up against something
				projectileSpawn.forward = projectileSpawn.parent.forward;
				return;
			}
			
			projectileSpawn.LookAt(hit.point);
			
		}
		else
		{
			// Ray didnt intersect with anything -> Just point forward
			projectileSpawn.forward = projectileSpawn.parent.forward;
		}
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(projectileSpawn.position, projectileSpawn.forward);
	}

	
}
