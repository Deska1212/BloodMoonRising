using UnityEngine;

/// <summary>
/// Sword is in this state while it is attacking
/// </summary>
public class SwordAttackingState : WeaponBaseState
{
	public SwordAttackingState()
	{
		stateName = "Attacking";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		// Play attack animation
		weapon.GetWeaponAnimator().SetTrigger("Attack");
		weapon.GetWeaponsController().CanSwitch = false;
	}

	public override void OnStateTick(Weapon weapon)
	{
		// Wait until the attack animation is finished to transition to recovery
		if (weapon.GetWeaponAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			weapon.ChangeState(new SwordRecoveringState());
		}
	}

	public override void OnStateExit(Weapon weapon)
	{
		
	}
}
