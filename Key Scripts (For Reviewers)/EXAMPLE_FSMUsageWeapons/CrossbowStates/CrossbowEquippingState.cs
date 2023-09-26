using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Crossbow is in this state while it is being equipped
/// </summary>
public class CrossbowEquippingState : WeaponBaseState
{

	public CrossbowEquippingState()
	{
		stateName = "Equipping";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		// Cast
		Crossbow crossbow = (Crossbow)weapon;
		
		// Check if string is already pulled back
		if (crossbow.loaded)
		{
			// String must be pulled back
			crossbow.PullbackString();
		}

	}

	public override void OnStateTick(Weapon weapon)
	{
		// Check to see if our equipping state has finished if so transition to next state
		if (weapon.GetWeaponAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			
			// Perform additional checks to move to either fired or ready state based on if we have a bolt loaded
			
			//
			RangedWeapon rangedWeapon = (RangedWeapon)weapon;
			
			// Check if the crossbow is loaded
			if (rangedWeapon.loaded)
			{
				// We have just equipped the crossbow and it already has a bolt in it so transition to ready state
				// Note that fired and ready are the same state in the animator
				Debug.Log("weapon is already loaded moving to ready state");

				weapon.GetWeaponAnimator().SetTrigger("Equip"); // Moves us instantly to ready state
				weapon.ChangeState(new CrossbowReadyState());
			}
			else
			{
				// We have equipped crossbow however it is not loaded - transition to fired and check for bolts
				Debug.Log("weapon is NOT loaded moving to fired state");
				weapon.GetWeaponAnimator().SetTrigger("Equip"); // Moves us instantly to fired state
				weapon.ChangeState(new CrossbowFiredState());
			}
		}
	}

	public override void OnStateExit(Weapon weapon)
	{
		
	}
}
