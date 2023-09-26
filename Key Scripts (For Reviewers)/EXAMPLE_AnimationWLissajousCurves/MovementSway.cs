using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Responsible for applying idle and movement reacting animations for the active weapon
/// Sway gets affected by input from InputManager
/// </summary>
public class MovementSway : MonoBehaviour
{
	[Header("General sway settings")]
	public bool active = true; // Is this sway active?
	[SerializeField] private float swayMultiplier; // Overall multiplier
	[SerializeField] private float perlinMultiplier; // Perlin noise multiplier
	[SerializeField] private float swaySmoothness; // Smoothness we use in smooth damp
	
	[Header("Idle sway settings")]
	[SerializeField] private float idleAmplitude;
	[SerializeField] private float idleFrequency;
	
	[Header("Walking sway settings")]
	[SerializeField] private float walkingAmplitude;
	[SerializeField] private float walkingFrequency;
	
	[Header("Sprinting sway settings")]
	[SerializeField] private float sprintingAmplitude;
	[SerializeField] private float sprintingFrequency;
	
	// Private fields
	private float currentAmplitude; // Current amplitude of the lissajous curve affected by player movement
	private float currentFrequency; // Current frequency of the lissajous curve affected by player movement
	private Vector3 smoothDampVelocity; // Velocity ref for smooth damp function
	private const float LISSAJ_RATIO = 0.5f; // Frequency ratio
	
	
	// References
	private PlayerMovementController playerMovementController; // We need this to check if we are sprinting
	private Transform activeWeaponModel; // Grab this when we switch weapons
	private Vector3 activeWeaponDefaultPos; // Our active weapons default position

	private float currentTime;
	
	#region Event Subscriptions

	private void OnEnable()
	{
		WeaponsController.OnWeaponSwitched += GetActiveWeaponTransform;
	}

	private void OnDisable()
	{
		WeaponsController.OnWeaponSwitched -= GetActiveWeaponTransform;
	}

	#endregion
	
	private void Awake()
	{
		playerMovementController = GetComponentInParent<PlayerMovementController>(); // This script is attatched as a child of Player - Can guarantee its there
		GetActiveWeaponTransform();
		
	}
	
	private void Update()
	{
		currentTime = Time.timeSinceLevelLoad;
		if (active)
		{
			AssignSwayValues();
			HandleSway();
		}
	}

	/// <summary>
	/// Applies sway smoothly to position
	/// </summary>
	private void HandleSway()
	{
		// Get our x and y values
		var x = LissajCurve(LISSAJ_RATIO);
		var y = LissajCurve(1f);

		// Turn into vec3 so we can use smoothdamp
		Vector3 sway = new Vector3(x, y, 0) * swayMultiplier;
		
		// Add some perlin noise
		float perlinX = Mathf.PerlinNoise(currentTime, currentTime);
		float perlinY = Mathf.PerlinNoise(currentTime + 10, currentTime + 10);

		// Normalise it between the range of -1 to 1 > PerlinNoise only gives us range 0 to 1
		perlinX = Mathf.Lerp(-1, 1, perlinX);
		perlinY = Mathf.Lerp(-1, 1, perlinY);

		// Add to sway with a multiplier
		sway.x += perlinX * perlinMultiplier;
		sway.y += perlinY * perlinMultiplier;
			
		
		// Apply our sway with smooth damp
		activeWeaponModel.localPosition = Vector3.SmoothDamp(activeWeaponModel.localPosition, activeWeaponDefaultPos + sway, ref smoothDampVelocity, swaySmoothness * Time.deltaTime);
		// activeWeaponModel.localPosition = Vector3.Lerp(activeWeaponModel.localPosition, activeWeaponDefaultPos + sway, swaySmoothness * Time.deltaTime);
	}

	/// <summary>
	/// Returns our lissaj curve
	/// </summary>
	/// <param name="ratio">Frequency ratio</param>
	private float LissajCurve(float ratio)
	{
		var liss = currentAmplitude * Mathf.Sin((currentFrequency * ratio) * currentTime); // Do i need to add an offset?
		return liss;
	}

	/// <summary>
	/// Assigns sway values based on input, whether we are idle, walking or sprinting
	/// </summary>
	private void AssignSwayValues()
	{
		// Simplified bool for if we are moving
		bool moving = InputManager.Instance.moveX != 0 || InputManager.Instance.moveY != 0;
		
		// Check if we are moving
		if (moving)
		{
			// We are definitely moving -> however we could be sprinting too so check and assign accordingly
			currentAmplitude = playerMovementController.IsSprinting ? sprintingAmplitude : walkingAmplitude; // If we are sprinting, sprinting amplitude, otherwise walking
			currentFrequency = playerMovementController.IsSprinting ? sprintingFrequency : walkingFrequency; // If we are sprinting, sprinting frequency, otherwise walking
		}
		else
		{
			// We are idle
			currentAmplitude = idleAmplitude;
			currentFrequency = idleFrequency;
		}
	}
	
	/// <summary>
	/// Grabs the active weapon object from weapons controller
	/// </summary>
	private void GetActiveWeaponTransform()
	{
		// Grab the active weapon AS a weapon
		Weapon currentWeapon = GetComponentInParent<WeaponsController>().GetCurrentWeapon();
		
		// Grab that active weapons transform
		activeWeaponModel = currentWeapon.gameObject.transform;
		
		// Grab the default position of that weapon that we will animate off
		activeWeaponDefaultPos = currentWeapon.defaultPosition;
	}
		
}
