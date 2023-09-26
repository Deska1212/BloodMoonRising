using UnityEngine;

/// <summary>
/// Base ranged weapon state
/// </summary>
public abstract class WeaponBaseState
{
	protected string stateName;
	
	public abstract void OnStateEnter(Weapon weapon);

	public abstract void OnStateTick(Weapon weapon);
	
	public abstract void OnStateExit(Weapon weapon);

	public string GetStateName()
	{
		return stateName;
	}
}