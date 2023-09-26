using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplayManager : UserInterfaceDisplayManager
{
	[SerializeField] private TextMeshProUGUI ammoCountText;
	[SerializeField] private float shakeVelocity = 0.035f;
	[SerializeField] private float shakeMagnitude = 3.5f;
	[SerializeField] private int shakeLoops = 2;
	
	
	
	public override void UpdateDisplay<T>(T value)
	{
		// Using 'dynamic' to cast to an int -> can ensure type safety through the event params
		dynamic ammoCount = value;
		
		// If we have to ammo left and we try reload - do a little UI jingle
		if (ammoCount == 0)
		{
			// Do UI jingle
			LeanTween.value(gameObject, RotationCallback, -shakeMagnitude, shakeMagnitude, shakeVelocity).setLoopPingPong(shakeLoops).setOnComplete(ReturnToDefaultRotation);	
		}

		ammoCountText.text = ammoCount.ToString();

	}
	
	private void ReturnToDefaultRotation()
	{
		LeanTween.value(gameObject, RotationCallback, gameObject.transform.localRotation.z, 0, shakeVelocity);
	}

	private void RotationCallback(float r)
	{
		gameObject.transform.localRotation = Quaternion.Euler(0, 0, r);
	}

	#region Event Subscriptions

	private void OnEnable()
	{
		AmmoController.OnBoltCountChanged += UpdateDisplay;
	}

	private void OnDisable()
	{
		AmmoController.OnBoltCountChanged -= UpdateDisplay;
	}

	#endregion
}
