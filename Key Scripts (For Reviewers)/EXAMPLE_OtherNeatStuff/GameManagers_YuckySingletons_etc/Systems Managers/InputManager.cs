using System;
using UnityEngine;

/// <summary>
/// This class is reposnible for handling input and letting it get retreived by objects
/// </summary>
public class InputManager : MonoBehaviour
{
	public static InputManager Instance;


	public bool canMove = true; // Master input switch
	
	// Cardinal directions
	public bool smoothAxis = false;
	public float moveX;
	public float moveY;

	public float mouseX;
	public float mouseY;

	public float mouseSensitivity;

	public Action OnJump;
	public Action OnSprintStart;
	public Action OnSprintRelease;

	public bool jumping;
	public bool sprinting;
	
	
	
	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (canMove)
		{
			HandleCardinalInput();
			HandleJumpInput();
			HandleSprintInput();
			HanldeMouseInput();
		}
	}

	private void HanldeMouseInput()
	{
		mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
		mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
	}

	private void HandleSprintInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Debug.Log("On Sprint Start Invoked");
			OnSprintStart?.Invoke();
		}
		
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			Debug.Log("On Sprint Release Invoked");
			OnSprintRelease?.Invoke();
		}

		sprinting = Input.GetKey(KeyCode.LeftShift);
	}

	private void HandleJumpInput()
	{
		if (Input.GetButtonDown("Jump"))
		{
			Debug.Log("On Jump Invoked");
			OnJump?.Invoke();
		}

		jumping = Input.GetButton("Jump");
	}

	private void HandleCardinalInput()
	{
		if (smoothAxis)
		{
			moveX = Input.GetAxis("Horizontal");
			moveY = Input.GetAxis("Vertical");
		}
		else
		{
			moveX = Input.GetAxisRaw("Horizontal");
			moveY = Input.GetAxisRaw("Vertical");
		}

	}
}
