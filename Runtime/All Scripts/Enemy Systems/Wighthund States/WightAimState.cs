using UnityEngine;


/// <summary>
/// State machine behaviour for wighthunds aiming behaviour
/// </summary>
public class WightAimState : MonsterBaseState
{

	private Transform playerTransform;
	private Wighthund wighthund;

	private float timer;
	
	public WightAimState()
	{
		stateName = "Aiming";
	}
	
	public override void OnStateEnter(Monster monster)
	{
		// Cache player transform for further use
		playerTransform = GameObject.FindWithTag("Player").transform;
		
		// Cast and cache this wighthund
		wighthund = (Wighthund)monster;

		// Immediately stop the navmesh agent
		wighthund.GetMonsterMovementController().GetNavMeshAgent().isStopped = true;
		
		// Assign timer
		timer = wighthund.attackWarmup;
		
		// Calculate our aim direction
		Vector3 chargeDirection = playerTransform.position - wighthund.transform.position; 
		
		// Store players direction as soon as we enter aim in wighthund
		wighthund.SetAimDirection(chargeDirection);
	}

	public override void OnStateTick(Monster monster)
	{
		timer -= Time.deltaTime;
		
		if (timer <= 0)
		{
			// Transition to charging state
			wighthund.ChangeState(new WightChargingState());
		}
	}

	public override void OnStateExit(Monster monster)
	{
		// Make sure we re-enable the agent after stopping it
		wighthund.GetMonsterMovementController().GetNavMeshAgent().isStopped = false;
	}
}
