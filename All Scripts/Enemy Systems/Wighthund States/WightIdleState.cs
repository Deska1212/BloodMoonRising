using UnityEngine;

/// <summary>
/// State machine state for wighthunds idle behaviour
/// Can transition to:
///		- Chase
///		- Wander
///
/// In idle, the wighthund will play an idle animation and stay in place for either a random amount of time
/// determined when we enter the state, or an indefinite amount of time (based on a bool, mainly for debug,
/// so the wight wont wander off)
///
/// After that pause time has elapsed, the wighthund will transition back into a wander state.
/// 
/// </summary>
public class WightIdleState : MonsterBaseState
{
	
	
	private float pauseTime; // How long this wighthund will pause in idle for - determined on state enter 
	private float timer; // Tick down from pauseTime to 0 -> Transition to wander @ 0

	private Transform playerTransform; // Needed for range check

	public WightIdleState()
	{
		stateName = "Idle";
	}

	public override void OnStateEnter(Monster monster)
	{
		// Cache the player transform for further use
		playerTransform = GameObject.FindWithTag("Player").transform;
		
		// Cast our monster parameter to a wighthund to access values - we can insure this monster is a wighthund
		Wighthund wight = (Wighthund)monster;
		
		// Grab a random value between our pause min and max
		pauseTime = GetPauseTime(wight);
		
		// Set our timer to the random pause time
		timer = pauseTime;
	}

	public override void OnStateTick(Monster monster)
	{
		// Cast monster TODO: Is casting this expensive? We are doing it every frame... Is it worth doing this in EnterState and caching
		Wighthund wighthund = (Wighthund)monster;
		
		// Check if the player is within chase distance
		if (ChaseDistanceCheck(wighthund))
		{
			// Transition to chase state -> Player is in range
			monster.ChangeState(new WightChaseState());
		}

		// Tick down our idle state timer
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			// Transition to a wander state -> Pause time has elapsed
			monster.ChangeState(new WightWanderState());
		}
	}

	public override void OnStateExit(Monster monster)
	{
		// Clean-up
	}

	private float GetPauseTime(Wighthund wight)
	{
		return Random.Range(wight.pauseTimeMin, wight.pauseTimeMax);
	}

	/// <summary>
	/// Returns true if player is within chase distance
	/// </summary>
	/// <returns></returns>
	private bool ChaseDistanceCheck(Wighthund wighthund)
	{
		return Vector3.Distance(playerTransform.position, wighthund.transform.position) <= wighthund.chaseDistance;
	}
}
