using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State machine state for charging behaviour
/// </summary>
// TODO: activate the charging trigger on the wighthund gameobject and apply damage to the player if charge is successful
// remember to deactivate the trigger as soon as charge is done or as soon as we hit the player to not apply
// duplicate damage
public class WightChargingState : MonsterBaseState
{
	private Transform playerTransform;
	private Wighthund wighthund;

	private Vector3 playerDirection;
	private Vector3 chargePosition;

	private NavMeshAgent agent;

	// The maximum amount of time this wighthund will spend in charge - ensure no weird behaviour because of unfulfilled destinations 
	private const float MAX_TIME_IN_CHARGE = 1.5f;
	private float timeInCharge;
	
	public WightChargingState()
	{
		stateName = "Charging";
	}
	
	
	public override void OnStateEnter(Monster monster)
	{
		// Grab and cache player transform for further use
		playerTransform = GameObject.FindWithTag("Player").transform;
		
		// Cast and cache wighthund from monster, we can ensure type security
		wighthund = (Wighthund)monster;

		// Cache this monsters navmesh agent as we are using it in update
		agent = wighthund.GetMonsterMovementController().GetNavMeshAgent();
		
		// Set agent speed
		agent.speed = wighthund.chargeSpeed;
		
		// Grab the direction toward the player
		playerDirection = wighthund.GetAimDirection();
		
		// Normalise player direction
		playerDirection.Normalize();
		
		// Calculate charge position TODO: turn this into a method
		chargePosition = wighthund.transform.position + playerDirection * wighthund.chargeDistance; // v + v * f
		
		// Set our nav mesh destination
		wighthund.GetMonsterMovementController().MoveToPoint(chargePosition);
		
		// Activate our charge collider trigger
		wighthund.chargeCollider.enabled = true;
		
		// Call OnCharge on Wighthund object
		wighthund.OnCharge();
		
		// Assign timer values
		timeInCharge = 0;
	}

	public override void OnStateTick(Monster monster)
	{
		// TODO: Is setting the nav agent destination the best way to do charging?

		// Increment time in charge by delta time each frame
		timeInCharge += Time.deltaTime;
		
		// Check if we have reached our nav mesh's destination
		bool reachedDestination = agent.remainingDistance <= agent.stoppingDistance;
		
		// Exit state once we have reached our charge destination
		if (reachedDestination)
		{
			// We have finished the charge -> Transition to Chase state, and set attack cooldown timer
			wighthund.ApplyAttackCooldown();
			wighthund.ChangeState(new WightChaseState());
		}

		if (timeInCharge >= MAX_TIME_IN_CHARGE)
		{
			// We have spent too long in charge, transition to chase
			wighthund.ApplyAttackCooldown();
			wighthund.ChangeState(new WightChaseState());
		}
	}

	public override void OnStateExit(Monster monster)
	{
		// De-activate our charge collider trigger
		wighthund.chargeCollider.enabled = false;
		
		
	}
	
	
}