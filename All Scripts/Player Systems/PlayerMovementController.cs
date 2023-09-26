using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class PlayerMovementController : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField] private float walkSpeed; // Standard walk speed
	[SerializeField] private float sprintSpeed; // Speed when holding shift, also represents our max speed

	[Header("Footstep settings")]
	[SerializeField] private float walkingFootstepInterval;
	[SerializeField] private float sprintingFootstepInterval;

	[Header("Jumping/Gravity Settings")]
	[SerializeField] private float jumpHeight; // Total jump height when doing big jump (spacebar held)
	[SerializeField] private float gravity; // Standard gravity, -9 is realistic but adjust for gameplay
	[SerializeField] private float ambientGravity = -1f; // Ambient gravity keeps player grounded, think of like friction to keep attached to ground
	[SerializeField] private float fallMultiplier; // Multiplier for gravity
	[SerializeField] private float lowJumpMultiplier; // Low jump gravity multiplier, raises gravity when performing smaller jumps (quick tapping spacebar)

	

	[Header("Stamina Settings")]
	[SerializeField] private float staminaRegeneration; // Stamina regen per second
	[SerializeField] private float staminaSprintDrain; // Stamina drained per second when sprinting
	[SerializeField] private float staminaJumpCost; // Stamina cost to jump
	
	// Actions/Events - These are public static so other classes can access them
	public static Action OnPlayerJump;
	// TODO: Static OnPlayerLand event when player lands from a jump


	// Private fields
	private float currentSpeed; // Controllers current speed based on input
	private float currentGravity; // Controllers current applied gravity
	private float staminaDelta; // Change in stamina per second
	private float yMove; // Controllers desired y Velocity based on gravity and situation (grounded, falling etc)
	private Vector3 move; // Movement Vector
	private Vector2 input; // Local input vector, x correlates to horizontal, y correlates to vertical and is subbed in as z movement, pulls this in method from
	private Vector3 velocity; // velocity for display purposes
	private float currentFootstepInterval; // Controllers current footstep interval base on whether we are walking or running

	// References
	private Player player;
	private CharacterController controller;

	// Hidden in inspector publics for PlayerMovementDebugUI
	// This is all yucky, TODO: Remove after player movement is fully implemented and balanced
	
	[HideInInspector] public float playerSpeed;
	[HideInInspector] public float currGravity;

	// Testing
	[SerializeField] private Vector3 knockback;
	[SerializeField] private float knockbackForce;
	[SerializeField] private float knockbackDecay;
	

	#region Movement Validations

	private bool isGrounded; // Correlates to controller is grounded can probably remove and just use properties
	private bool isSprinting; // Correlates to player sprint INPUT. i.e. if the player is holding down sprint key, NOT if actually sprinting in game
	[SerializeField] private float knockbackThreshold;

	// Implemented as auto props for public access
	
	public bool IsGrounded => isGrounded;
	public bool IsSprinting => isSprinting;

	public Vector3 Velocity => velocity;

	#endregion
	
	

	
	private void Awake()
	{
		// Pull refs
		
		player = GetComponent<Player>();
		controller = GetComponent<CharacterController>();
		
	}

	private void Start()
	{
		InputManager.Instance.OnJump += HandleJump;
		StartCoroutine(Footstep());
	}

	private void Update()
	{
		// Set readable velocity
		velocity = controller.velocity;
		isGrounded = ValidateGrounded();
		
		HandleMovement();
		HandleGravity();
		HandleSprinting();
		HandleStamina();
		HandleFootsteps();
		
		


		//TODO: Redunant after player movement is implemented
		UpdateDebugInfo();
	}

	

	/// <summary>
	/// Responsible for updating isSprinting, which tracks IF the player is ACTUALLY sprinting
	/// Not if they are holding shift
	/// </summary>
	private void HandleSprinting()
	{
		// isSprinting tracks whether we ARE ACTUALLY sprinting
		// in order to be considered sprinting we must have
		// - Input
		// - To be moving
		// - Enough stamina
		// - To be grounded
		isSprinting = InputManager.Instance.sprinting && velocity.magnitude > 0 && player.Stamina > 0 && isGrounded;
	}

	private void HandleStamina()
	{
		// TODO: calculate stamina delta here but pass it to the player to be deducted and clamped.
		// So we are not as tightly coupled
		
		// If we are sprinting drain, else regen
		staminaDelta = InputManager.Instance.sprinting && velocity.magnitude > 0 ? staminaSprintDrain : staminaRegeneration;

		if (isGrounded)
		{
			// Only calculate stamina if we are grounded
			player.Stamina += staminaDelta * Time.deltaTime;
		}
		
		// Clamp stamina
		player.Stamina = Mathf.Clamp(player.Stamina, 0, player.MaxStamina);
	}

	private void UpdateDebugInfo()
	{
		playerSpeed = velocity.magnitude;
		currGravity = currentGravity;
	}
	
	/// <summary>
	/// Handles application of player gravity based on situation
	/// </summary>
	private void HandleGravity()
	{
		bool falling = !isGrounded && velocity.y < 0;
		
		
		if (falling)
		{
			// If we are falling apply gravity harder
			currentGravity = gravity * fallMultiplier;
		}
		else if(!isGrounded && velocity.y > 0 && !InputManager.Instance.jumping)
		{
			// If we are still going up from our jump, but we have released the jump button, apply gravity harder as it is just a tap jump
			currentGravity = gravity * lowJumpMultiplier;
		}
		else if(!isGrounded)
		{
			// Default gravity, we are falling but still holding spacebar
			currentGravity = gravity;
		}
		else
		{
			// We are grounded, apply ambient gravity so we keep attached to the ground
			currentGravity = ambientGravity;
			
			// Clamp yMove while we are grounded so gravity doesnt add up
			yMove = Mathf.Clamp(yMove, -1f, Mathf.Infinity);
		}

		// Actually add gravity, note that must be multiplied by dt twice as it is metres per sec per sec (Acceletation not a velocity)
		yMove += currentGravity * Time.deltaTime;

		
	}

	private void HandleJump()
	{
		// Check to see if player has the stamina to jump and is on the ground
		bool hasStamina = player.Stamina - staminaJumpCost > 0;
		
		// TODO: Would be good to make the stamina bar flesh red if the player tries to jump but has no stam as a visual indicators
		if (!hasStamina)
		{
			return;
		}

		bool canJump = isGrounded;
		if (canJump)
		{
			// Sqrt func to move us just enough that we get our actual jump height (I think this works?)
			yMove = Mathf.Sqrt(-2f * jumpHeight * ambientGravity);
			player.Stamina -= staminaJumpCost;
			OnPlayerJump?.Invoke();

		}
	}


	/// <summary>
	/// Validates whether the player is on the ground
	/// </summary>
	private bool ValidateGrounded()
	{
		return controller.isGrounded;
	}

	/// <summary>
	/// Handles movement, done through Unity built in char contr
	/// </summary>
	private void HandleMovement()
	{
		
		// Turning input values into a vector so i can easily clamp the magnitude
		input.x = InputManager.Instance.moveX;
		input.y = InputManager.Instance.moveY;
		
		input = Vector2.ClampMagnitude(input, 1);

		// Assign current speed based on if we are sprinting
		currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

		// Little bit of vector stuff ensures we are moving locally.
		move = transform.right * input.x + transform.forward * input.y;
		
		// Add y after matrix stuff so doesn't get multiplied by 0
		move.y = yMove;
		
		// // Testing knockback
		// if (Input.GetKeyDown(KeyCode.Z))
		// {
		// 	knockback = -transform.forward * knockbackForce;
		// }
		//
		// if (knockback.magnitude > knockbackThreshold)
		// {
		// 	knockback = Vector3.Lerp(knockback, Vector3.zero, Time.deltaTime * knockbackDecay);
		// 	controller.Move(new Vector3(knockback.x, yMove, knockback.z));
		// }
		// else
		// {
		// 	
		// }

		// Tell controller to move as normal
		controller.Move(move * (currentSpeed * Time.deltaTime));
		
	}
	
	/// <summary>
	/// Responsible for handling player footsteps
	/// </summary>
	private void HandleFootsteps()
	{
		// Assign footstep interval
		currentFootstepInterval = isSprinting ? sprintingFootstepInterval : walkingFootstepInterval;
		
		// Check if we are moving and grounded
		bool stepping = velocity.magnitude > 0.1f && IsGrounded;

	}
	
	
	//TODO: Convert this to a timer
	private IEnumerator Footstep()
	{
		if (velocity.magnitude > 0.1f && isGrounded)
		{
				// Instantiate footstep object
				GetComponent<FootstepController>().LeftFootstep();
				
				
		}
		// Wait for interval
		yield return new WaitForSeconds(currentFootstepInterval);
		
		StartCoroutine(Footstep());

	}


}
