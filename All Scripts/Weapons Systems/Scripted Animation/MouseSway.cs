using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a rotation to the active weapon to subtly move weapons in the players camera rotation direction
/// </summary>
public class MouseSway : MonoBehaviour
{
	[Header("General mouse sway settings")]
	public bool active = true;
	
	[SerializeField] private float mouseSwayMultiplier;
	[SerializeField] private float inputMax;
	[SerializeField] private float tiltMultiplier;
	[SerializeField] private float smoothing;

	
	// Refs
	private Transform activeWeaponModel; // We grab this every time we switch weapons

	private void Awake()
	{
		GetActiveWeaponTransform();
	}

	private void OnEnable()
	{
		WeaponsController.OnWeaponSwitched += GetActiveWeaponTransform;
	}

	private void OnDisable()
	{
		WeaponsController.OnWeaponSwitched -= GetActiveWeaponTransform;
	}

	private void Update()
	{
		if(active)
			HandleMouseSway();
	}

	private void HandleMouseSway()
	{	
		// Grab our mouse axis' and multiply them by our sway multipliers
		float inputX = InputManager.Instance.mouseX * mouseSwayMultiplier;
		float inputY = InputManager.Instance.mouseY * -mouseSwayMultiplier; // Has to be negative
		
		// Clamp max movement amounts, probably best to this as private variables to reduce clutter
		inputX = Mathf.Clamp(inputX, -inputMax, inputMax);
		inputY = Mathf.Clamp(inputY, -inputMax, inputMax);

		// Handle seperate rotation axis
		Quaternion rotX = Quaternion.AngleAxis(inputY, Vector3.right); // Handle up/down reaction to mouse input
		Quaternion rotY = Quaternion.AngleAxis(inputX, Vector3.up); // Handle left/right reaction to mouse input
		Quaternion rotZ = Quaternion.AngleAxis(-InputManager.Instance.moveX * tiltMultiplier, Vector3.forward); // Handle left/right tilt reaction to movement

		// Handle target rotation by multiplying all those vectors
		Quaternion targetRot = rotX * rotY * rotZ;

		// Slerp the weapon handles current rotation to the target rotation
		activeWeaponModel.localRotation = Quaternion.Slerp(activeWeaponModel.localRotation, targetRot, smoothing * Time.deltaTime);
	}

	private void GetActiveWeaponTransform()
	{
		activeWeaponModel = GetComponentInParent<WeaponsController>().GetCurrentWeapon().gameObject.transform;
	}
}
