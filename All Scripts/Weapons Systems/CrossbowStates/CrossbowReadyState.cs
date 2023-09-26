using System.Linq.Expressions;
using UnityEngine;

/// <summary>
/// Crossbow is in this state when it is ready to be fired
/// </summary>
public class CrossbowReadyState : WeaponBaseState
{
	private const string FIRE_TRIGGER = "Fire"; // Trigger name for fire on animator

	private Animator wingAnimator;
	
	public CrossbowReadyState()
	{
		stateName = "Ready";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		
	}

	public override void OnStateTick(Weapon weapon)
	{
		// Downcast weapon to a ranged weapon to access functionality - note that we CAN GUARANTEE this is a ranged weapon
		RangedWeapon rangedWeapon = (RangedWeapon)weapon;
		
		// Look for lmb input and fire a projectile
		if (Input.GetMouseButtonDown(0) && rangedWeapon.loaded)
		{
			// Instantiate a bolt -> bolt gets it velocity when it gets instantiated from Projectile class
			rangedWeapon.Fire();
			
			
			// Trigger firing animation
			rangedWeapon.GetWeaponAnimator().SetTrigger(FIRE_TRIGGER);

			// Remove the bolt that was loaded
			rangedWeapon.loaded = false;

			// Move crossbow to fired state
			rangedWeapon.ChangeState(new CrossbowFiredState());
		}
	}

	
	public override void OnStateExit(Weapon weapon)
	{
		
	}

	private void FireBolt()
	{
		
	}
}
