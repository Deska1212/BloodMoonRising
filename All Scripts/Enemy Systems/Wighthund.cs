using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Wighthund class derived as: Entity -> Monster -> Wighthund
/// </summary>
public class Wighthund : Monster
{
	[Header("Wander state settings")]
	
	[Tooltip("The speed that this wighthund wanders")]
	public float wanderSpeed;
	[Tooltip("How far this wighthund samples distances to wander to")]
	public float wanderDistance;
	
	[Header("Idle state settings")]
	
	[Tooltip("The minimum time this wighthund pauses for idle state")]
	public float pauseTimeMin;
	[Tooltip("The maximum time this wighthund pauses for idle state")]
	public float pauseTimeMax;
	
	[Header("Chase state settings")]
	
	[Tooltip("The distance the player must be under for this wighthund to chase")]
	public float chaseDistance;
	[Tooltip("The speed that this wighthund chases the player")]
	public float chaseSpeed;
	[Tooltip("The rotational speed this wighthund turns toward the player when it cannot attack but is in attack range")]
	public float rotationSpeed;
	
	[Header("Attacking settings")]
	
	[Tooltip("The distance this wighthund travels when it charges.")]
	public float chargeDistance;
	[Tooltip("The speed this wighthund travels when it charges.")]
	public float chargeSpeed;
	[Tooltip("The distance the wighthund can initiate an attack under")]
	public float attackRange;
	[Tooltip("The damage the wighthund applies to the object it attacks")]
	public float attackDamage;
	[Tooltip("If this wighthund can attack")]
	public bool canAttack;
	[Tooltip("How long the wighthund takes to aim when initiating an attack")]
	public float attackWarmup;
	[Tooltip("How long after the wighthund has attacked to wait until it can attack again")]
	public float attackCooldown;
	
	// Private fields
	private Vector3 initialPosition;
	private Vector3 chargeDirection;
	private float attackCooldownTimer;
	
	[Header("Effects")]
	[SerializeField] private GameObject deathParticleEffect;
	[SerializeField] private GameObject chargeParticleEffect;
	[SerializeField] private AudioClip chargeSoundEffect;
	
	// References
	[SerializeField] public Collider chargeCollider; 
	
//TODO; change stopping distance per state

	protected override void Awake()
	{
		base.Awake();
		initialPosition = transform.position;
	}
	
	private void Start()
	{
		//TODO: Temporary state stuff
		ChangeState(new WightIdleState());
		Init();
	}

	/// <summary>
	/// Responsible for assigning default values for this monster
	/// </summary>
	private void Init()
	{
		// Assign default values
		movement.GetNavMeshAgent().speed = wanderSpeed; // This is changed per state in state enter (e.g. chase state will assign chase speed)
		
	}

	protected override void Update()
	{
		base.Update();
		attackCooldownTimer -= Time.deltaTime;

		if (attackCooldownTimer <= 0 && canAttack == false)
		{
			canAttack = true;
		}
	}

	public Vector3 GetInitialPosition()
	{
		return initialPosition;
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(initialPosition, new Vector3(0.3f, 3f, 0.3f)); // Initial position flag
			
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(initialPosition, wanderDistance); // How far wighthund will wander

			switch (state)
			{
				case WightIdleState:
					Gizmos.DrawLine(initialPosition, transform.position); // Draw line to fence post 
					break;
				case WightWanderState: 
					Gizmos.DrawLine(initialPosition, transform.position); // Draw line to location we are wandering to
					break;
				case WightChaseState:
					Gizmos.color = Color.red;
					Gizmos.DrawLine(movement.GetNavMeshAgent().destination, transform.position); // Draw line to player (destination)
					break;
				case WightChargingState:
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(movement.GetNavMeshAgent().destination, transform.position); // Draw line to charge destination
					break;
			}
		}
	}

	public void ApplyAttackCooldown()
	{
		attackCooldownTimer = attackCooldown;
		canAttack = false;
	}

	public Vector3 GetAimDirection()
	{
		return chargeDirection;
	}
	
	public void SetAimDirection(Vector3 dir)
	{
		chargeDirection = dir;
	}

	private void OnTriggerEnter(Collider other)
	{
		// Turn off charge collider
		chargeCollider.enabled = false;

		// Check if our collision is the player
		if (other.gameObject.CompareTag("Player"))
		{
			// Get the collisions objects damageable interface
			IDamageable damageable = other.GetComponent<IDamageable>();
			
			// Apply damage to the collision object if it is damageable and the player
			if (damageable != null)
			{
				damageable.TakeDamage(attackDamage);
				Debug.Log(monsterName + " has applied" + attackDamage + " damage to " + other.name);
				
				// Stop the wighthund from charging and apply the attack cooldown
				ApplyAttackCooldown();
				ChangeState(new WightChaseState());
			}	
		}
	}

	/// <summary>
	/// Called from state machine when this Wighthund initiates a charge
	/// </summary>
	public void OnCharge()
	{
		// Play charge sfx
		ChargeEffects();
	}

	public override void Die()
	{
		SpawnDeathFX();
		base.Die();
	}

	/// <summary>
	/// Used in boss fights -> We have alot of Wighthunds all dying at once at the end of a boss fight
	/// This can cause one big audio play which is a hearing risk, 
	/// </summary>
	public void DieWithoutSFX()
	{
		SpawnDeathFX();
		Destroy(this.gameObject);
	}

	private void SpawnDeathFX()
	{
		Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
	}

	private void ChargeEffects()
	{
		// Play charge sound
		audioSource.PlayOneShot(chargeSoundEffect);
		
		// Instantiate charge particle system
		Instantiate(chargeParticleEffect, transform.position, transform.rotation);
	}
}
