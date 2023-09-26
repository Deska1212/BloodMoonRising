using System.Collections;
using UnityEngine;

/// <summary>
/// Crossbow is in this state while it is reloading, we cannot shoot or switch weapons
/// </summary>
public class CrossbowReloadingState : WeaponBaseState
{
	

	public CrossbowReloadingState()
	{
		stateName = "Reloading";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		// Pull a bolt, play the reload animation and wait until we have finished the reload animation to transition to ready
		
		// Cast weapon as a crossbow - I think this is very yucky im not sure...
		RangedWeapon crossbow = (RangedWeapon)weapon;
		
		// Disable switching
		crossbow.GetWeaponsController().CanSwitch = false;

		// Remove the bolt we are loading from the inventory
		crossbow.GetAmmoController().RemoveBolt();
		
		Debug.Log("Reloading crossbow");
		
	}

	public override void OnStateTick(Weapon weapon)
	{

		// Check if the current reload state is dont and we are not in transition
		if (weapon.GetWeaponAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !weapon.GetWeaponAnimator().IsInTransition(0))
		{
			// Reload animation is done so move to the ready state
			weapon.GetWeaponAnimator().SetTrigger("Reloaded");
			weapon.ChangeState(new CrossbowReadyState());
		}
	}
	
	public override void OnStateExit(Weapon weapon)
	{
		RangedWeapon crossbow = (RangedWeapon)weapon;
		crossbow.loaded = true;
		weapon.GetWeaponsController().CanSwitch = true;
	}
}
