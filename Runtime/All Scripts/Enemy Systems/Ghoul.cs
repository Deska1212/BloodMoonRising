using System;
using UnityEngine;

/// <summary>
/// Ghoul class derived as: Entity -> Monster -> Ghoul
/// </summary>
public class Ghoul : Monster
{
	[Header("Ghoul state settings")]
	
	[Tooltip("Speed that this Ghoul chases the player")]
	public float chaseSpeed;
	[Tooltip("Distance where the Ghoul can initiate an attack")]
	public float attackRange;
	[Tooltip("Damage this Ghoul applies on attack")]
	public float attackDamage;
	
	[Header("Attack Settings")]

	[Tooltip("If this Ghoul can attack")]
	public bool canAttack;
	[Tooltip("How long after attacking this Ghoul has to wait until it can attack again")]
	public float attackCooldown;
	[Tooltip("The point at which our overlap sphere is created")]
	[SerializeField] private Transform attackPoint;
	[Tooltip("The radius at which our attack overlap sphere extends at attack animation event")]
	public float attackRadius;

	[Tooltip("The speed that this ghoul faces the player when he is in attack range but cannot attack")]
	public float rotationSpeed;

	[Header("Effects")]
	[SerializeField] private GameObject deathFX;
	[SerializeField] private GameObject attackFX;
	[SerializeField] private AudioClip attackSound;

	[Header("Boss fight Settings")]
	public bool isInvulnerable;

	[Header("Debugging Settings")]
	[SerializeField] private bool drawGizmos;
	
	// Private fields
	private float attackCooldownTimer;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		ChangeState(new GhoulIdleState());
	}

	protected override void Update()
	{
		attackCooldownTimer -= Time.deltaTime;

		if (attackCooldownTimer <= 0)
		{
			canAttack = true;
		}

		base.Update();
	}

	/// <summary>
	/// Creates an overlap sphere to an area and applies damage to damageable player objects in that area.
	/// </summary>
	public void Attack()
	{
		// Apply attack cooldown
		ApplyAttackCooldown();
		
		// Create an attack fx
		Instantiate(attackFX, transform.position, transform.rotation);
		PlayAttackSounds();
		
		// Create an overlap sphere
		Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

		foreach (var collider in colliders)
		{
			if (collider.CompareTag("Player"))
			{
				// Check if the player is within overlap sphere, apply damage if so
				IDamageable damageable = collider.GetComponent<IDamageable>();
				damageable.TakeDamage(attackDamage);
			}
		}



	}

	

	public void ApplyAttackCooldown()
	{
		canAttack = false;
		attackCooldownTimer = attackCooldown;
	}

	private void SpawnDeathFX()
	{
		Instantiate(deathFX, transform.position, Quaternion.identity);
	}
	
	private void PlayAttackSounds()
	{
		audioSource.PlayOneShot(attackSound);
	}

	public override void TakeDamage(float dmg)
	{
		if (!isInvulnerable)
		{
			// Only take damage if this ghoul is currently not invulnerable.
			base.TakeDamage(dmg);
		}
	}

	public override void Die()
	{
		SpawnDeathFX();
		// TODO: need to do some stuff with boss manager here
		base.Die();
	}

	public void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			// Draw attack radius
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackRange);
		
			// Draw attack point
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(attackPoint.position, attackRadius);	
		}
	}
	
	
}
