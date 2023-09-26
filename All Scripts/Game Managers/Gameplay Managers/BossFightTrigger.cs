using System;
using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
	// The boss fight instance this trigger is tied to
	public BossFightManager bossFightManager; 
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// Activate the boss fight
			bossFightManager.InitiateBossFight();
			
			// Deactivate this trigger
			GetComponent<Collider>().enabled = false;
		}
	}
}
