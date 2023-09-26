using UnityEngine;


/// <summary>
/// Crossbow is in this state while it is being unequipped.
/// </summary>
public class CrossbowUnequippingState : WeaponBaseState
{
	public CrossbowUnequippingState()
	{
		stateName = "Unequipping";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		// Start playing unequipping animation
		weapon.GetWeaponAnimator().SetTrigger("Unequip");
	}

	public override void OnStateTick(Weapon weapon)
	{
		
	}

	public override void OnStateExit(Weapon weapon)
	{
		
	}
}
