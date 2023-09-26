using UnityEngine;

/// <summary>
/// Base state for monster AI state machine
/// </summary>
[System.Serializable]
public abstract class MonsterBaseState
{
	protected string stateName;
	
	public abstract void OnStateEnter(Monster monster);

	public abstract void OnStateTick(Monster monster);
	
	public abstract void OnStateExit(Monster monster);

	public string GetStateName()
	{
		return stateName;
	}
}
