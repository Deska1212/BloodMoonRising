using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
	[SerializeField] private bool active;

	[SerializeField] private List<Wighthund> activeWighthunds;
	[SerializeField] private GameObject wighthundPrefab;

	// Maximum wighthunds this spawner can spawn, this is done per spawner to keep spawns localised,
	// i.e. if you kill a wighthund in an area, it should respawn in that area.
	[SerializeField] private int maxActiveWighthunds; 

	[SerializeField] private float timeBetweenSpawns;

	private float spawnTimer;

	private void Awake()
	{
		spawnTimer = timeBetweenSpawns;
	}

	private void Update()
	{
		// If this spawner is active, tick down the spawn timer
		if (active)
		{
			// Tick down timer
			spawnTimer -= Time.deltaTime;
		}
		
		// If the timer elapses, try spawn a wighthund
		if (spawnTimer <= 0)
		{
			ResetTimer();
			
			// Try spawn a wighthund if we can
			if (activeWighthunds.Count < maxActiveWighthunds)
			{
				// We can spawn a wighthund
				SpawnWighthund();
			}
		}

		CleanUpActiveList();
	}

	private void CleanUpActiveList()
	{
		for (int i = 0; i < activeWighthunds.Count; i++)
		{
			if (activeWighthunds[i] == null)
			{
				// Remove null wighthund from the list then break
				activeWighthunds.RemoveAt(i);
			}
		}
	}

	private void ResetTimer()
	{
		// Reset time plus some small random factor (So spawns arent at the same time for each spawner)
		spawnTimer = timeBetweenSpawns + Random.Range(-2f, 2f);
	}

	private void SpawnWighthund()
	{
		Debug.Log("Spawning Wighthund!");
		// Instantiate a wighthund
		GameObject wighthundGameObject = Instantiate(wighthundPrefab, transform.position, Quaternion.identity);
		Wighthund wighthund = wighthundGameObject.GetComponent<Wighthund>();
		activeWighthunds.Add(wighthund);
	}

	public void ActivateSpawner()
	{
		active = true;
	}

	public void DeactivateSpawner()
	{
		active = false;
	}

	public int GetActiveWighthundCount()
	{
		return activeWighthunds.Count;
	}

	public void DestroyAllWighthunds()
	{
		// Loop through the list - calling a function on each active wighthund that kills it then clear the list
		foreach (Wighthund wighthund in activeWighthunds)
		{
			wighthund.DieWithoutSFX();
		}
	}

	
}
