using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Crossbow is in Fired state when it is unloaded, from here we can automatically load if we
/// have any ammo
///
/// If we have just transitioned to fired from ready, wait a half second or so before automatically reloading.
/// This gives the player the opportunity to switch weapons, and it doesn't look janky with an immediate reload
/// after firing.
/// </summary>
public class CrossbowFiredState : WeaponBaseState
{
	
	protected float recoveryTime = 1.0f; // Time after transition into state
	protected float timer;
	protected bool recovered = false;

	public CrossbowFiredState()
	{
		stateName = "Fired";
	}

	public override void OnStateEnter(Weapon weapon)
	{
		
	}

	public override void OnStateTick(Weapon weapon)
	{
		timer += Time.deltaTime;
		
		// Set recovered to true after recovery time has elapsed.
		if (timer > recoveryTime)
		{
			recovered = true;
		}

		if (recovered)
		{
			// Check for bolts each frame
			CheckForBolts(weapon);
		}
	}
	
	private void CheckForBolts(Weapon weapon)
	{
		// Pull crossbow from weapon
		RangedWeapon crossbow = (RangedWeapon)weapon;
		if (crossbow.GetAmmoController().Bolts > 0)
		{
			// We have bolts, and we aren't already reloading -> Transition to reloading state
			weapon.GetWeaponAnimator().SetTrigger("Reload");
			weapon.ChangeState(new CrossbowReloadingState());
		}

	}

	public override void OnStateExit(Weapon weapon)
	{
		
	}
}
