using System;
using UnityEngine;

/// <summary>
/// Sword class
/// </summary>
public class Sword : MeleeWeapon
{
	
	
	public Sword()
	{
		weaponName = "Sword";
	}

	private void Awake()
	{
		
	}

	private void OnEnable()
	{
		// Grab animators here as awake is only called on gameobjects that initialise in the scene in an active state
		animator = GetComponentInChildren<Animator>();
		weaponsController = GetComponentInParent<WeaponsController>(); 
		ChangeState(new SwordEquippingState());
	}

	public override void Equip()
	{
		ChangeState(new SwordEquippingState());
	}

	public override void Unequip()
	{
		ChangeState(new SwordUnequippingState());
	}

	public override void Ready()
	{
		ChangeState(new SwordReadyState());
	}
	
	
	
}
