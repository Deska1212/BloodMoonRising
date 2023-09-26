using System;
using UnityEngine;
public class AmmoBox : MonoBehaviour, IInteractable
{
	// Temporary material change implementation
	
	[SerializeField] private Material defaultMaterial;
	[SerializeField] private Material selectedMaterial;

	private Renderer ammoboxRenderer;

	[SerializeField] private GameObject AmmoBoxOpenedPrefab;
	

	private void Start()
	{
		ammoboxRenderer = GetComponentInChildren<Renderer>();
	}

	public void Select()
	{
		ammoboxRenderer.sharedMaterial = selectedMaterial;
	}

	public void Deselect()
	{
		ammoboxRenderer.sharedMaterial = defaultMaterial;
	}

	public void Interact()
	{
		// Snuff out this lamppost 
		GetComponentInParent<LampPost>().SnuffOut();
		
		// Provide the player with ammo
		GameObject.FindWithTag("Player").GetComponent<AmmoController>().RefillAmmo();
		
		// Instantiate opened ammo box prefab at exact same location
		Instantiate(AmmoBoxOpenedPrefab, transform.position, transform.rotation);
		
		// Destroy this gameobject
		Destroy(this.gameObject);
	}
}