using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Relic : SceneObject
{
	public BossFightManager bossFightManager;
	
	public float vertexCount;
	public float noise;
	public float pointBHeight;

	
	[SerializeField] private Transform lineRendererStartPosition;
	private LineRenderer lineRenderer;
	private MeshFilter meshFilter;
	private Light light;
	
	private Vector3 pointA;
	private Vector3 pointB;
	private Vector3 pointC;

	[Header("Effects")]
	[SerializeField] private GameObject hitVFX;
	[SerializeField] private GameObject destroyFX;

	[SerializeField] private GameObject brokenRelicModel;
	[SerializeField] private GameObject relicModel;

	
	[SerializeField] private AudioClip[] hitSFX;

	[SerializeField] private Transform hitParticlePoint; // Assign in inspector 
	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		meshFilter = GetComponentInChildren<MeshFilter>();
		light = GetComponentInChildren<Light>();
	}

	public override void Die()
	{
		// Call back to the boss fight manager to remove us from list
		bossFightManager.RemoveRelicFromList(this);
		
		// Disable the old relic model gameobject
		relicModel.SetActive(false);
		
		// Swap the model gameobject prefab to the broken variant
		Instantiate(brokenRelicModel, relicModel.transform.position, relicModel.transform.rotation);
		
		// Disable point light
		light.enabled = false;
		
		// Disable line renderer
		lineRenderer.enabled = false;
		
		// Spawn a visual effect and play some audio
		DestroyFX();
	}

	private void DestroyFX()
	{
		// Create a fx gameobject
		GameObject sfx = Instantiate(destroyFX, hitParticlePoint.position, Quaternion.identity);
	}

	public override void TakeDamage(float dmg)
	{
		// Spawn a visual effect and play some audio
		HitFX();
		
		base.TakeDamage(dmg);
	}


	private void HitFX()
	{
		GameObject hitParticle = Instantiate(hitVFX, hitParticlePoint.position, Quaternion.identity);
		
		int rand = Random.Range(0, hitSFX.Length);
		AudioClip clip = hitSFX[rand];
		
		GetComponent<AudioSource>().PlayOneShot(clip);
	}

	private void Update()
	{
		if(bossFightManager.active)
			Draw();
	}

	private void Draw()
	{
		// Vector3 directionToBoss = transform.position - pointC;
		pointA = transform.position;
		pointC = bossFightManager.ghoul.transform.position;

		pointB = new Vector3((pointA.x + pointC.x) / 2f + UnityEngine.Random.Range(-0.1f, 0.1f), pointBHeight + UnityEngine.Random.Range(-0.1f, 0.1f), (pointA.z + pointB.z) / 2f + UnityEngine.Random.Range(-0.1f, 0.1f));

		List<Vector3> pointsList = new List<Vector3>();

		for (float ratio = 0f; ratio <= 1f; ratio += 1f/vertexCount)
		{
			Vector3 tangetA = Vector3.Lerp(pointA, pointB, ratio);
			Vector3 tangetB = Vector3.Lerp(pointB, pointC, ratio);
			Vector3 curve = Vector3.Lerp(tangetA, tangetB, ratio);

			curve.x += (UnityEngine.Random.Range(-noise, noise) * Time.deltaTime);
			curve.y += (UnityEngine.Random.Range(-noise, noise) * Time.deltaTime);
			curve.z += (UnityEngine.Random.Range(-noise, noise) * Time.deltaTime);

			pointsList.Add(curve);


		}

        

		lineRenderer.positionCount = pointsList.Count;
		lineRenderer.SetPositions(pointsList.ToArray());
	}
}
