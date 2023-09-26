using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class is repsonsible for handling weapon switching and input logic.
/// Weapon switching will be done with the 'Q' key. That iterates through a list of weapons
/// If the last weapon is selected and we go to the next it wraps back to the first weapon.
///
/// There is a delay when switching weapons
///
/// 
/// </summary>
public class WeaponsController : MonoBehaviour
{
	// Store a list of weapons we can switch between
	public List<Weapon> weapons;

	[SerializeField] private bool canSwitch;
	[SerializeField] private int currentWeaponIndex = 0;
	
	

	public static Action OnWeaponSwitched; // TODO: Make this return the weapon we switched to

	public bool CanSwitch
	{
		get => canSwitch;
		set => canSwitch = value;
	}

	private void Update()
	{
		// TODO: Check if we only have one weapon if we do don't bother switching
		if (Input.GetKeyDown(KeyCode.Q) && CanSwitch)
		{
			// Player has switched weapon - call the internal switch weapon function and broadcast a switch weapon event for any listeners
			SwitchWeapon();
			
		}
	}

	private void SwitchWeapon()
	{
		// Call a coroutine that de-instantiates the current weapon after its unequip animation has finished (the de-instantiation is handled via the weapon itself)
		// and instantiates the new weapon, playing the equip animation on awake
//test
		StartCoroutine(SwitchWeaponSequence());

	}

	private IEnumerator SwitchWeaponSequence()
	{
		CanSwitch = false;
		Weapon currentWeapon = weapons[currentWeaponIndex];
		
		
		
		// TODO: Refactor this so equip and unequip functions out their correlating animation time
		// E.g. Equip(out equipAnimationTime), and halt for that time
		
		// Play our put weapon away animation on the currently equip weapon by switching state
		currentWeapon.Unequip();

		// Pull unequip animation time from current weapon we are unequipping
		float tA = currentWeapon.GetWeaponAnimator().GetCurrentAnimatorClipInfo(0).Length;

		
		
		// Halt this coroutine until animation is done
		yield return new WaitForSeconds(tA);

		
		
		// Deactivate the weapon and make sure we wont get dependency/null ref errors
		DeactivateWeapon(currentWeapon);
		
		
		
		// Index through weapons list, checking to see if we are at the end and looping back if need be
		if (currentWeaponIndex == weapons.Count - 1)
		{
			// We are at the end of the list, wrap back
			currentWeaponIndex = 0;
		}
		else
		{
			++currentWeaponIndex;
		}

		// Update the new current weapon as our index has changed
		currentWeapon = weapons[currentWeaponIndex];

		// This is pretty expensive
		if (currentWeapon == null)
		{
			// Freak the fuck out if we dont have a current weapon
			Debug.LogError("Current weapon is null, check your index logic in WeaponsController?");
		}

		// Activate the new weapon in hierarchy - if we cant find it we should instantiate it?
		ActivateWeapon(currentWeapon);
		
		// Set the new weapon to the equipping state
		currentWeapon.Equip();


		// Pull animation time
		float tB = currentWeapon.GetWeaponAnimator().GetCurrentAnimatorClipInfo(0).Length;
		
		// Move to ready state TODO: Remember we have to do some loaded checks here
		OnWeaponSwitched?.Invoke();
		

		// Yield return until animation is done, this is just so we can remain isSwitching
		yield return new WaitForSeconds(tB);
		
		CanSwitch = true;
		
		
	}

	/// <summary>
	/// Deactivates a weapon in the hierarchy
	/// </summary>
	private void DeactivateWeapon(Weapon weapon)
	{
		weapon.gameObject.SetActive(false);
	}

	/// <summary>
	/// Activates a weapon in the hierarchy
	/// </summary>
	private void ActivateWeapon(Weapon weapon)
	{
		weapon.gameObject.SetActive(true);
	}

	public Weapon GetCurrentWeapon()
	{
		return weapons[currentWeaponIndex];
	}
}
