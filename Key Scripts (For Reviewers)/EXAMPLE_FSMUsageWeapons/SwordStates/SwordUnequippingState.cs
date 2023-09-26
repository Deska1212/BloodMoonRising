using UnityEngine;

/// <summary>
/// Sword is in this state while it is being unequipped
/// </summary>
public class SwordUnequippingState : WeaponBaseState
{
	public SwordUnequippingState()
	{
		stateName = "Unequipping";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		// Play the unequip animation
		weapon.GetWeaponAnimator().SetTrigger("Unequip");
	}

	public override void OnStateTick(Weapon weapon)
	{
		
	}

	public override void OnStateExit(Weapon weapon)
	{
		
	}
}
