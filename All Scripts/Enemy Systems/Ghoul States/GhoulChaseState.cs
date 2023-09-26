using UnityEngine;
using UnityEngine.AI;

public class GhoulChaseState : MonsterBaseState
{
	private Transform playerTransform;
	private Ghoul ghoul;
	
	public override void OnStateEnter(Monster monster)
	{
		// Cache our casted type
		ghoul = (Ghoul)monster;
		
		// Cache our player transform
		playerTransform = GameObject.FindWithTag("Player").transform;
		
		// Set our chase speed
		ghoul.GetMonsterMovementController().GetNavMeshAgent().speed = ghoul.chaseSpeed;
	}

	public override void OnStateTick(Monster monster)
	{
		NavMeshAgent ghoulAgent = ghoul.GetMonsterMovementController().GetNavMeshAgent();

		// If we are in range but we can't attack we must just face the player
		if (AttackRangeCheck() && !ghoul.canAttack)
		{
			ghoulAgent.updateRotation = true;
			FacePlayer();
		}
		else
		{
			ghoulAgent.updateRotation = true;
		}

		// Check if we are inside attack range and we can attack, transition to aim if so
		if (AttackRangeCheck() && ghoul.canAttack)
		{
			// Transition to aim state
			monster.ChangeState(new GhoulAttackState());
		}
		
		ChasePlayer();
	}

	

	public override void OnStateExit(Monster monster)
	{
		// Should immediately stop the navmesh agent
		ghoul.GetMonsterMovementController().GetNavMeshAgent().SetDestination(ghoul.transform.position);
	}
	
	private void ChasePlayer()
	{
		ghoul.GetMonsterMovementController().GetNavMeshAgent().SetDestination(playerTransform.position);
	}
	
	/// <summary>
	/// Returns true if the player is within attack range
	/// </summary>
	private bool AttackRangeCheck()
	{
		// Check if player is under attack range
		return Vector3.Distance(playerTransform.position, ghoul.transform.position) <= ghoul.attackRange;
	}
	
	private void FacePlayer()
	{
		Vector3 lookPos = playerTransform.position - ghoul.transform.position;
		lookPos.y = 0;
		Quaternion rot = Quaternion.LookRotation(lookPos);
		ghoul.transform.rotation = Quaternion.Slerp(ghoul.transform.rotation, rot,  ghoul.rotationSpeed * Time.deltaTime);
	}
}
