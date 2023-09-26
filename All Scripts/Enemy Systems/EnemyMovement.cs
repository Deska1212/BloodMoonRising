using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// This class is responsible for moving the enemy player using
/// unity's NavMesh
/// </summary>
public class EnemyMovement : MonoBehaviour
{
	// Refs/Dependencies
	private NavMeshAgent agent; // TODO: Return this for other classes through a method

	/// <summary>
	/// Gets a random destination for a navmesh agent from a fixed point
	/// </summary>
	/// <param name="point">Point to get destination from</param>
	/// <param name="radius">Radius around point to sample destination</param>
	/// <returns>A vector3 between 0 and radius away in a random direction from point</returns>
	public Vector3 GetRandomDestinationFromPoint(Vector3 point, float radius)
	{
		// Get a random unit direction 
		Vector3 randomDirection = Random.insideUnitSphere;
		
		// Get a random distance from the point from 0 to radius 
		Vector3 randomPosRelativeToPoint = randomDirection * Random.Range(0, radius) + point;
		
		// Handle navmesh
		NavMeshHit hit;
		
		// Initialise a pos variable
		Vector3 finalPosition = Vector3.zero;
		
		// Sample a position from a navmesh
		if(NavMesh.SamplePosition(randomPosRelativeToPoint, out hit, radius, NavMesh.AllAreas))
		{
			finalPosition = hit.position;
		}
		return finalPosition; 
	}

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	public void MoveToPoint(Vector3 point)
	{
		agent.SetDestination(point);
	}

	public NavMeshAgent GetNavMeshAgent()
	{
		return agent;
	}
}
