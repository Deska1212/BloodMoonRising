using UnityEngine;

/// <summary>
/// Sword is in this state while it is in recovery
/// </summary>
public class SwordRecoveringState : WeaponBaseState
{
	public SwordRecoveringState()
	{
		stateName = "Recovering";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		weapon.GetWeaponAnimator().SetTrigger("Recover");
	}

	public override void OnStateTick(Weapon weapon)
	{
		// The animator transition to ready state is automatic, check if we are still in the
		// Recovering animation, if we are not we are back in ready state
		if (!weapon.GetWeaponAnimator().GetCurrentAnimatorStateInfo(0).IsName("Recovering"))
		{
			weapon.ChangeState(new SwordReadyState());
		}
	}

	public override void OnStateExit(Weapon weapon)
	{
		weapon.GetWeaponsController().CanSwitch = true;
	}
}
