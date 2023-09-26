using System;
using UnityEngine;

	public class LineRendPan : MonoBehaviour
	{
		private LineRenderer renderer;
		[SerializeField] private float speed;

		private void Start()
		{
			renderer = GetComponent<LineRenderer>();
		}

		private void Update()
		{
			float offset = Time.time * speed;
			renderer.material.mainTextureOffset = new Vector2(offset, 0);
		}
	}
