using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class BossTrackerDisplayManager : UserInterfaceDisplayManager
{

	// Each individual UI element that represents a boss
	public Image[] bossTrackerDisplayComponents;

	[SerializeField] private float tweenInTime;
	[SerializeField] private float tweenOutTime;
	[SerializeField] private Vector2 tweenedInPosition;
	[SerializeField] private float tweenedInScale;
	[SerializeField] private float tweenOutDelay;

	private Vector2 defaultPosition; // Using rect transform element
	private float defaultScale; // Scale is assumed to be uniform
	private RectTransform rectTransform; // This gameobjects rectTrasform
	
	public Sprite bossDeadSprite;
	public Sprite bossAliveSprite;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		defaultPosition = rectTransform.anchoredPosition;
		defaultScale = rectTransform.localScale.x;
	}
	
	


	/// <summary>
	/// Gets called when the OnBossFightEnded event is fired
	/// </summary>
	public override void UpdateDisplay<T>(T value)
	{
		// Cast with dynamic, grabbing an integer from event
		dynamic currentBossCount = value;

		// Tween into position and scale, after that is done, update the UI element
		LeanTween.value(gameObject, PositionCallback, rectTransform.anchoredPosition, tweenedInPosition, tweenInTime);
		LeanTween.value(gameObject, ScaleCallback, rectTransform.localScale.x, tweenedInScale, tweenInTime);
		
		for (int i = 0; i < GameManager.DEFAULT_BOSS_COUNT; ++i)
		{
			if (i < currentBossCount)
			{
				// This boss is still active, 
				bossTrackerDisplayComponents[i].sprite = bossAliveSprite;
			}
			else if (i >= currentBossCount)
			{
				// This boss has been killed, deactivate the tracker image
				StartCoroutine(ActivateDisplayElementAtIndex(i));
			}
		}
		
	}

	private void TweenOut()
	{
		LeanTween.value(gameObject, PositionCallback, rectTransform.anchoredPosition, defaultPosition, tweenOutTime);
		LeanTween.value(gameObject, ScaleCallback, rectTransform.localScale.x, defaultScale, tweenOutTime);
	}


	#region Callbacks for scale and position

	private void PositionCallback(Vector2 pos)
	{
		rectTransform.anchoredPosition = pos;
	}
	
	private void ScaleCallback(float s)
	{
		rectTransform.localScale = new Vector2(s, s);
	}

	#endregion


	
	/// <summary>
	/// I have to do this weirdly as the function that is hooked up to generic event can only pass the param
	/// </summary>
	/// <param name="idx">Index to refresh - the icon owned by the boss that was just killed</param>
	private IEnumerator ActivateDisplayElementAtIndex(int idx)
	{
		yield return new WaitForSeconds(tweenInTime + (tweenOutDelay / 2f));

		BossIndicatorShakeFeedback(bossTrackerDisplayComponents[idx].gameObject);
		bossTrackerDisplayComponents[idx].sprite = bossDeadSprite;
		
		yield return new WaitForSeconds(tweenOutDelay / 2f);
		TweenOut();
	}

	private void BossIndicatorShakeFeedback(GameObject component)
	{
		Vector3 defaultScale = component.transform.localScale;
		LeanTween.scale(component, defaultScale * 1.25f, 0.5f).setLoopPingPong(1);
	}

	#region Event Subscriptions

	private void OnEnable()
	{
		BossFightManager.OnBossFightEnded += UpdateDisplay;
	}

	private void OnDisable()
	{
		BossFightManager.OnBossFightEnded -= UpdateDisplay;
	}

	#endregion
}
