using UnityEngine;

/// <summary>
/// Sword is in this state when it is ready and can attack
/// </summary>
public class SwordReadyState : WeaponBaseState
{
	public SwordReadyState()
	{
		stateName = "Ready";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		
	}

	public override void OnStateTick(Weapon weapon)
	{
		// Check for input if we attack transition to attack state
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("MBD");
			weapon.ChangeState(new SwordAttackingState());
		}
	}

	public override void OnStateExit(Weapon weapon)
	{
		
	}
}
