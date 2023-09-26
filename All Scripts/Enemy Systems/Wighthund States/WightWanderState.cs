using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State machine state for wighthunds wandering behaviour
/// Can transition to:
///		- Idle
///		- Chase
///
/// In wander, the wighthund will get a destination from to wander to from a fixed
/// location where it started (If it is a level design placed wighthund)
///
/// Wighthunds that spawn in boss fights will be a special prefab that has a large enough
/// chase distance to cover the whole boss fight, thus wander is irrelevant in that use case.
///
/// After reaching its destination, the wighthund will transition to idle for an amount of type
/// (the range of which is defined in the Wighthund class). Once that idle time has elapsed,
/// the wighthund will transition back to wander, repeating the process.
/// </summary>
public class WightWanderState : MonsterBaseState
{
	private Vector3 randomWanderPoint;
	private Transform playerTransform;
	private Wighthund wighthund;

	public WightWanderState()
	{
		stateName = "Wandering";
	}

	public override void OnStateEnter(Monster monster)
	{
		// Cast our monster to a wighthund as cache - we can ensure type
		wighthund = (Wighthund)monster;
		
		// Cache the players transform
		playerTransform = GameObject.FindWithTag("Player").transform;
		
		// Grab reference to movement controller
		EnemyMovement movement = wighthund.GetMonsterMovementController();
		
		// Grab and store a random point to wander to
		randomWanderPoint = movement.GetRandomDestinationFromPoint(wighthund.GetInitialPosition(), wighthund.wanderDistance);
		
		// Set our wander speed
		wighthund.GetMonsterMovementController().GetNavMeshAgent().speed = wighthund.wanderSpeed;
	}

	public override void OnStateTick(Monster monster)
	{
		Wander(monster);
		if (ChaseDistanceCheck())
		{
			// Transition to chase state -> Player is in range
			monster.ChangeState(new WightChaseState());
		}
	}

	public override void OnStateExit(Monster monster)
	{
		
	}

	private void Wander(Monster monster)
	{

		// Grab reference to our movement controller
		EnemyMovement movement = monster.GetMonsterMovementController();
		
		// Move to our point
		movement.MoveToPoint(randomWanderPoint);
		
		// Simplified bool returns true if we are already wandering
		bool pathComplete = movement.GetNavMeshAgent().pathStatus == NavMeshPathStatus.PathComplete;
		bool withinCompleteDistance = movement.GetNavMeshAgent().remainingDistance <= movement.GetNavMeshAgent().stoppingDistance;
		bool reachedDestination = pathComplete && withinCompleteDistance;
		
		// Guard clause if we are already wandering, return
		if (reachedDestination)
		{
			// We have reached our destination -> Transition to idle for some time
			monster.ChangeState(new WightIdleState());
		}
	}

	/// <summary>
	/// Returns true if player is within chase distance
	/// </summary>
	/// <returns></returns>
	private bool ChaseDistanceCheck()
	{
		return Vector3.Distance(playerTransform.position, wighthund.transform.position) <= wighthund.chaseDistance;
	}
}
