using UnityEngine;

/// <summary>
/// Sword is in this state while it is being equipped
/// </summary>
public class SwordEquippingState : WeaponBaseState
{
	public SwordEquippingState()
	{
		stateName = "Equipping";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		
	}

	public override void OnStateTick(Weapon weapon)
	{
		// Check if we have finished our equip animation
		if (weapon.GetWeaponAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			weapon.GetWeaponAnimator().SetTrigger("Equip"); // Moves us to ready state
			weapon.ChangeState(new SwordReadyState());
		}
	}

	public override void OnStateExit(Weapon weapon)
	{
		
	}
}
