using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is placed on a child object of a lamppost and is responsible
/// for getting its parent lamppost and snuffing it out when player walks
/// into attached trigger. The game object attached to this class
/// also needs a trigger collider of some sort
/// </summary>
[RequireComponent(typeof(Collider))]
public class LampPostTrigger : MonoBehaviour
{
	private LampPost parentLampPost;
	
	private void Awake()
	{
		parentLampPost = GetComponentInParent<LampPost>();
	}

	private void OnTriggerEnter(Collider other)
	{
		// Guard if we haven't any parent lamp post for whatever reason
		if (parentLampPost == null)
		{
			return;
		}
		
		// Check if the collided object was a player
		if (other.gameObject.CompareTag("Player"))
		{
			// TODO: Refactor this into a Unity event or action
			// Call snuff on parent lamp post
			// this would be much better as an event or action
			parentLampPost.SnuffOut();
		}
	}
}
