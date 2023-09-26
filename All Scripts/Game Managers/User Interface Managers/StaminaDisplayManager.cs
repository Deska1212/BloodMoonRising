using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplayManager : UserInterfaceDisplayManager
{
	private Slider staminaBar;

	private void Awake()
	{
		staminaBar = GetComponent<Slider>();
	}

	public override void UpdateDisplay<T>(T value)
	{
		// Cast generic to a float
		dynamic stamina = value;
		
		// Update our stamina display
		staminaBar.value = (float)stamina;
	}

	#region Event Subscriptions

	private void OnEnable()
	{
		Player.OnStaminaChanged += UpdateDisplay;
	}

	private void OnDisable()
	{
		Player.OnStaminaChanged -= UpdateDisplay;
	}

	#endregion
}
