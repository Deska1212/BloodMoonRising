using System;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for updating the health bar
/// </summary>
[RequireComponent(typeof(Slider))]
public class HealthDisplayManager : UserInterfaceDisplayManager
{
	private Slider healthBar;

	private void Awake()
	{
		healthBar = GetComponent<Slider>();
	}

	

	public override void UpdateDisplay<T>(T value)
	{
		// Cast our generic to a float using dynamic
		dynamic health = value;

		// Update health display
		healthBar.value = (float)health;
	}

	#region Event Subscriptions

	private void OnEnable()
	{
		Player.OnHealthChanged += UpdateDisplay;
	}

	private void OnDisable()
	{
		Player.OnHealthChanged -= UpdateDisplay;
	}

	#endregion
}
