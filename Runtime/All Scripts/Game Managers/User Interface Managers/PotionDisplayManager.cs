using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionDisplayManager : UserInterfaceDisplayManager
{
	

	[SerializeField] private TextMeshProUGUI potionCountText;
	[SerializeField] private float shakeVelocity = 0.1f;
	[SerializeField] private float shakeMagnitude = 4f;
	[SerializeField] private int shakeLoops = 4;
	
	private void Awake()
	{
		
	}

	

	public override void UpdateDisplay<T>(T value)
	{
		Debug.Log("Updating potion count");
		dynamic potionCount = value; // TODO: ?
		

		// Check if we have no potions and do a little UI jingle if we dont - Feedback to player that they're out of potions 
		if (potionCount == 0)
		{
			// Do UI jingle
			LeanTween.value(gameObject, RotationCallback, -shakeMagnitude, shakeMagnitude, shakeVelocity).setLoopPingPong(shakeLoops).setOnComplete(ReturnToDefaultRotation);	
		}

		potionCountText.text = potionCount.ToString();
	}

	private void ReturnToDefaultRotation()
	{
		LeanTween.value(gameObject, RotationCallback, gameObject.transform.localRotation.z, 0, shakeVelocity);
	}

	private void RotationCallback(float r)
	{
		gameObject.transform.localRotation = Quaternion.Euler(0, 0, r);
	}





	#region Events Subscriptions

	private void OnEnable()
	{
		// Subscribe to events
		Player.OnPotionCountChanged += UpdateDisplay;
	}

	private void OnDisable()
	{
		// Unsubscribe to events
		Player.OnPotionCountChanged -= UpdateDisplay;
	}

	#endregion
}
