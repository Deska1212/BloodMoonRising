using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State machine state for wighthunds chase behaviour
/// </summary>
public class WightChaseState : MonsterBaseState
{
	private Transform playerTransform;
	private Wighthund wighthund;

	public WightChaseState()
	{
		stateName = "Chasing";
	}

	public override void OnStateEnter(Monster monster)
	{
		// Cache our casted type
		wighthund = (Wighthund)monster;
		
		// Ensure the navmesh agent is enabled on this wighthund
		wighthund.GetMonsterMovementController().GetNavMeshAgent().enabled = true;
		
		// Cache player transform
		playerTransform = GameObject.FindWithTag("Player").transform;
		
		// Set our chase speed
		wighthund.GetMonsterMovementController().GetNavMeshAgent().speed = wighthund.chaseSpeed;
	}

	public override void OnStateTick(Monster monster)
	{
		// Attack distance GUARD clause
		
		// Check if we are inside attack range and we can attack, transition to aim if so
		// ELSE Check if we are already in aim distance but cant attack, if so just look at the player
		if (AttackRangeCheck() && wighthund.canAttack)
		{
			// Transition to aim state
			monster.ChangeState(new WightAimState());
		}
		
		

		NavMeshAgent wighthundAgent = wighthund.GetMonsterMovementController().GetNavMeshAgent();
		
		if (wighthundAgent.remainingDistance < wighthundAgent.stoppingDistance)
		{
			// Do manual rotation here if we are too close
			wighthundAgent.updateRotation = false;
			FacePlayer();
		}
		else
		{
			wighthundAgent.updateRotation = true;
		}



		
	

		// Check if we are outside of chase range * a small modifier to prevent rapid switching of states now that we are in chase
		if (!ChaseDistanceCheckWithThreshold())
		{
			// Transition back to idle
			monster.ChangeState(new WightIdleState());
		}
		
		if (AttackRangeCheck() && !wighthund.canAttack)
		{
			wighthund.GetMonsterMovementController().GetNavMeshAgent().SetDestination(wighthundAgent.transform.position);
			return;
		}
		
		// Chase the player
		ChasePlayer();
	}

	private void FacePlayer()
	{
		Vector3 lookPos = playerTransform.position - wighthund.transform.position;
		lookPos.y = 0;
		Quaternion rot = Quaternion.LookRotation(lookPos);
		wighthund.transform.rotation = Quaternion.Slerp(wighthund.transform.rotation, rot,  wighthund.rotationSpeed * Time.deltaTime);
	}


	public override void OnStateExit(Monster monster)
	{
		
	}

	/// <summary>
	/// Use EnemyMovement to chase the player
	/// </summary>
	private void ChasePlayer()
	{
		// Set this wighthunds destination to be the player
		// TODO: This is being done every frame which is kinda yucky so find a way to optimise, I probably also need some sort of validation here
		wighthund.GetMonsterMovementController().GetNavMeshAgent().SetDestination(playerTransform.position);
	}
	
	/// <summary>
	/// Returns true if the player is within attack range
	/// </summary>
	private bool AttackRangeCheck()
	{
		// Check if player is under attack range
		return Vector3.Distance(playerTransform.position, wighthund.transform.position) <= wighthund.attackRange;
	}

	/// <summary>
	/// Returns true if player is within chase distance
	/// </summary>
	/// <returns></returns>
	private bool ChaseDistanceCheckWithThreshold()
	{
		// Check if we are outside of chase range plus a small modifier to prevent us from rapidly switching on the edge of states
		return Vector3.Distance(playerTransform.position, wighthund.transform.position) <= wighthund.chaseDistance * 1.25f;
	}
}
