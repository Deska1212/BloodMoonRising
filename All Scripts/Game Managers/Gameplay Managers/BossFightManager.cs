using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles instances of boss fights
/// </summary>
public class BossFightManager : MonoBehaviour
{
	public bool active; // Is there a boss fight going on
	public GameObject ghoul; // The ghoul object tied to this boss fight
	
	[Header("Trigger settings")]
	[Header("Trigger settings")]
	public Collider entranceTrigger;
	public Collider entranceBlocker;
	public Collider exitBlocker;

	[Header("Boss Encounter Elements")]
	// Spawners list
	public List<Spawner> spawners;

	// Relics list
	public List<Relic> relics;
	
	[Header("Boss Encounter Audio")]
	
	// Boss audio
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private float musicVolume;
	[SerializeField] private float musicFadeTime;
	
	
	// Events
	public static Action<int> OnBossFightEnded; // Fires when boss fight has ended, passes through new # of active bosses

	private void Update()
	{
		// Check if ghoul has been killed
		if (ghoul == null && active == true)
		{
			// Ghoul is dead
			CompleteBossFight();
		}
	}

	public void InitiateBossFight()
	{
		// Enable entrance blocker
		EnableEntranceBlocker();

		// Activate boss fight
		active = true;
		
		// Activate Ghoul
		ghoul.GetComponent<Ghoul>().ChangeState(new GhoulChaseState());
		
		// Start boss music
		BossMusicFadeIn();
		
		// Activate spawners
		foreach (Spawner spawner in spawners)
		{
			spawner.ActivateSpawner();
		}

	}

	public void CompleteBossFight()
	{
		// Set active to false
		active = false;
		
		// Deactivate exit blocker
		DisableExitBlocker();
		
		// Start boss music
		BossMusicFadeOut();
		

		// Deactivate spawners & Kill all wighthunds
		foreach (Spawner spawner in spawners)
		{
			spawner.DeactivateSpawner();
			spawner.DestroyAllWighthunds();
		}
		
		// Update new number of bosses
		GameManager.instance.activeBosses--;
		
		// Invoke boss fight complete event
		OnBossFightEnded?.Invoke(GameManager.instance.activeBosses);
	}


	#region Audio

	private void BossMusicFadeIn()
	{
		// Start the audio source
		musicSource.Play();
		
		// Tween in boss audio
		LeanTween.value(this.gameObject, MusicFadeCallback, musicSource.volume, musicVolume, musicFadeTime);
	}
	
	private void BossMusicFadeOut()
	{
		// Tween out boss audio
		LeanTween.value(this.gameObject, MusicFadeCallback, musicVolume, 0f, musicFadeTime).setOnComplete(() => musicSource.Stop());
	}
	
	private void MusicFadeCallback(float v)
	{
		musicSource.volume = v;
	}

	#endregion
	
	

	private void EnableEntranceBlocker()
	{
		// TODO: This should be a nice animation of the forest closing behind the player rather than just an invisible wall
		entranceBlocker.enabled = true;
	}

	private void DisableExitBlocker()
	{
		exitBlocker.enabled = false;
	}

	public void RemoveRelicFromList(Relic relic)
	{
		relics.Remove(relic);
		
		// Clear ghoul invulnerability if we have no more relics
		if (relics.Count == 0)
		{
			ClearGhoulInvulnerability();
		}
	}
	
	public void ClearGhoulInvulnerability()
	{
		ghoul.GetComponent<Ghoul>().isInvulnerable = false;
	}
}
